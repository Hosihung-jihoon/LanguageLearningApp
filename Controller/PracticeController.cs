using LanguageLearningApp.Data;
using LanguageLearningApp.DTOs;
using LanguageLearningApp.Models;
using LanguageLearningApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageLearningApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class PracticeController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly AiService _aiService;

    public PracticeController(AppDbContext context, AiService aiService)
    {
        _context = context;
        _aiService = aiService;
    }

    // 1. Lấy danh sách chủ đề
    [HttpGet("topics")]
    public async Task<IActionResult> GetTopics()
    {
        var topics = await _context.Topics
            .Select(t => new { t.Id, t.Name, t.Description })
            .ToListAsync();
        return Ok(topics);
    }

    // 2. Lấy danh sách câu hỏi theo chủ đề và loại (câu đơn hoặc hội thoại)
    [HttpGet("sentences/{topicId}")]
    public async Task<IActionResult> GetSentences(int topicId, [FromQuery] string type = "Single", [FromQuery] int count = 10)
    {
        var isConversation = string.Equals(type, "Conversation", StringComparison.OrdinalIgnoreCase);
        if (isConversation)
        {
            var conversationIds = await _context.Sentences
                .Where(s => s.TopicId == topicId && s.Type == "Conversation" && s.ConversationId != null)
                .Select(s => s.ConversationId)
                .Distinct()
                .ToListAsync();

            if (!conversationIds.Any())
            {
                return NotFound("Chưa có đoạn hội thoại nào trong chủ đề này.");
            }

            var randomId = conversationIds[Random.Shared.Next(conversationIds.Count)];
            var sentences = await _context.Sentences
                .Where(s => s.ConversationId == randomId)
                .OrderBy(s => s.Id)
                .Select(s => new SentenceDto { Id = s.Id, VietnameseText = s.VietnameseText })
                .ToListAsync();

            return Ok(sentences);
        }
        else
        {
            var sentences = await _context.Sentences
                .Where(s => s.TopicId == topicId && (s.Type == "Single" || s.Type == "" || s.Type == null))
                .OrderBy(r => Guid.NewGuid())
                .Take(count)
                .Select(s => new SentenceDto { Id = s.Id, VietnameseText = s.VietnameseText })
                .ToListAsync();

            if (!sentences.Any())
            {
                // Fallback to any sentences in this topic if no explicit "Single" sentences found
                sentences = await _context.Sentences
                    .Where(s => s.TopicId == topicId)
                    .OrderBy(r => Guid.NewGuid())
                    .Take(count)
                    .Select(s => new SentenceDto { Id = s.Id, VietnameseText = s.VietnameseText })
                    .ToListAsync();
            }

            if (!sentences.Any())
            {
                return NotFound("Chưa có câu hỏi nào trong chủ đề này.");
            }

            return Ok(sentences);
        }
    }

    // 3. Nộp câu dịch và nhận kết quả AI
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitTranslation([FromBody] SubmitDto dto)
    {
        // Tìm câu hỏi gốc trong DB
        var sentence = await _context.Sentences.FindAsync(dto.SentenceId);
        if(sentence == null) return NotFound("Không tìm thấy câu hỏi.");

        // Gọi AI Service chấm điểm
        var feedback = await _aiService.GradeTranslationAsync(sentence.VietnameseText, dto.UserTranslation);
        if (feedback == null)
        {
            return StatusCode(500,"Không thể chấm điểm.");
        }

        // Lưu lịch sử học vào DB
        var attempt = new UserAttempt
        {
            SentenceId = dto.SentenceId,
            UserTranslation = dto.UserTranslation,
            AiFeedback = System.Text.Json.JsonSerializer.Serialize(feedback),
            MistakeType = feedback.MistakeType,
            CreatedAt = DateTime.UtcNow
        };

        _context.UserAttempts.Add(attempt);
        await _context.SaveChangesAsync();

        // Trả về cả AttemptId, Feedback và Phiên âm cho Frontend
        return Ok(new 
        { 
            Feedback = feedback, 
            AttemptId = attempt.Id,
            Phonetic = sentence.Phonetic
        });
    }

    // 4. Lấy đề xuất cuối set học
    [HttpPost("session-feedback")]
    public async Task<IActionResult> GetSessionFeedback([FromBody] SessionFeedbackDto dto)
    {
        if (dto.AttemptIds == null || !dto.AttemptIds.Any())
        {
            return BadRequest("Danh sách AttemptIds trống.");
        }

        // Lấy lịch sử làm bài từ DB dựa trên danh sách ID
        var attempts = await _context.UserAttempts
            .Where(ua => dto.AttemptIds.Contains(ua.Id))
            .ToListAsync();

        if (!attempts.Any())
        {
            return NotFound("Không tìm thấy lịch sử làm bài.");
        }

        // Trích xuất lỗi sai và giải thích từ AiFeedback (JSON) để gửi cho AI
        var mistakeSummaries = new List<string>();
        foreach (var attempt in attempts)
        {
            // Parse lại JSON từ AiFeedback để lấy thông tin lỗi
            var feedback = System.Text.Json.JsonSerializer.Deserialize<AiFeedbackDto>(attempt.AiFeedback);
            if (feedback != null && !feedback.IsCorrect)
            { 
                mistakeSummaries.Add($"Câu gốc: '{attempt.UserTranslation}'| Lỗi: {feedback.MistakeType} | Ghi chú: {feedback.MistakeExplanation}");
            }
            else if (feedback != null && feedback.IsCorrect)
            {
                mistakeSummaries.Add($"Câu đúng: '{attempt.UserTranslation}'");
            }
        }

        // Gọi AI Service tổng hợp
        var recommendations = await _aiService.GetSessionRecommendationsAsync(mistakeSummaries);

        return Ok(new { recommendations });
    }

    // 5. Lấy thống kê lịch sử học tập (Profile)
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var attempts = await _context.UserAttempts
            .Include(ua => ua.Sentence)
            .OrderByDescending(ua => ua.CreatedAt)
            .ToListAsync();

        var total = attempts.Count;
        if (total == 0)
        {
            return Ok(new
            {
                TotalAttempts = 0,
                CorrectCount = 0,
                AccuracyRate = 0,
                MistakeBreakdown = new Dictionary<string, int>(),
                History = new List<object>()
            });
        }

        var correctCount = 0;
        var breakdown = new Dictionary<string, int>();
        var historyList = new List<object>();

        foreach (var attempt in attempts)
        {
            var isCorrect = false;
            var correctedSentence = "";
            try 
            {
                var feedback = System.Text.Json.JsonSerializer.Deserialize<AiFeedbackDto>(attempt.AiFeedback);
                if (feedback != null)
                {
                    isCorrect = feedback.IsCorrect;
                    correctedSentence = feedback.CorrectedSentence ?? "";
                }
            }
            catch {}

            if (isCorrect) correctCount++;

            var mistakeType = string.IsNullOrEmpty(attempt.MistakeType) ? "None" : attempt.MistakeType;
            if (breakdown.ContainsKey(mistakeType))
            {
                breakdown[mistakeType]++;
            }
            else
            {
                breakdown[mistakeType] = 1;
            }

            historyList.Add(new
            {
                attempt.Id,
                attempt.UserTranslation,
                attempt.MistakeType,
                attempt.CreatedAt,
                IsCorrect = isCorrect,
                VietnameseText = attempt.Sentence?.VietnameseText ?? "Câu hỏi đã bị xóa",
                CorrectedSentence = correctedSentence,
                Phonetic = attempt.Sentence?.Phonetic
            });
        }

        var accuracy = (double)correctCount / total * 100;

        return Ok(new
        {
            TotalAttempts = total,
            CorrectCount = correctCount,
            AccuracyRate = Math.Round(accuracy, 1),
            MistakeBreakdown = breakdown,
            History = historyList,
            GeminiUsage = new
            {
                CurrentRpm = GeminiRateLimiter.CurrentRpm,
                CurrentRpd = GeminiRateLimiter.CurrentRpd,
                MaxRpm = GeminiRateLimiter.MaxRpm,
                MaxRpd = GeminiRateLimiter.MaxRpd
            }
        });
    }

    // 6. Lấy phân tích chi tiết kĩ năng cần cải thiện từ AI
    [HttpGet("improvement-recommendations")]
    public async Task<IActionResult> GetImprovementRecommendations()
    {
        // Lấy 20 lượt làm bài bị sai gần nhất để phân tích
        var attempts = await _context.UserAttempts
            .OrderByDescending(ua => ua.CreatedAt)
            .ToListAsync();

        var mistakeSummaries = new List<string>();
        var incorrectCount = 0;

        foreach (var attempt in attempts)
        {
            try 
            {
                var feedback = System.Text.Json.JsonSerializer.Deserialize<AiFeedbackDto>(attempt.AiFeedback);
                if (feedback != null)
                {
                    if (!feedback.IsCorrect)
                    {
                        incorrectCount++;
                        if (incorrectCount <= 15) // Phân tích tối đa 15 lỗi gần nhất để tránh quá tải token
                        {
                            mistakeSummaries.Add($"Câu dịch lỗi: '{attempt.UserTranslation}' | Loại lỗi: {feedback.MistakeType} | Chi tiết: {feedback.MistakeExplanation}");
                        }
                    }
                }
            }
            catch {}
        }

        if (!mistakeSummaries.Any())
        {
            return Ok(new { Recommendations = "### Rất tuyệt vời!\nBạn chưa mắc lỗi sai nào trong lịch sử làm bài. Hãy tiếp tục phát huy!" });
        }

        // Gọi AI Service để phân tích và đưa ra đề xuất cải thiện
        var recommendations = await _aiService.GetSessionRecommendationsAsync(mistakeSummaries, isShort: true);

        return Ok(new { Recommendations = recommendations });
    }
}