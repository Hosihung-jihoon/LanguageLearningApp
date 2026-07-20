using System.Text;
using System.Text.Json;
using LanguageLearningApp.DTOs;

namespace LanguageLearningApp.Services;

public class AiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public AiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<AiFeedbackDto?> GradeTranslationAsync(string vietnameseText, string userTranslation)
    {
        if (!GeminiRateLimiter.CheckAndIncrement(out var limitError))
        {
            throw new GeminiRateLimitException(limitError ?? "Hạn mức API Gemini đã vượt giới hạn.");
        }

        string apiKey = _config["GeminiApi:ApiKey"] ?? throw new Exception("Missing Gemini API Key");
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.1-flash-lite:generateContent?key={apiKey}";

        // Thiết kế Prompt để ép AI trả về json chuẩn
        string systemPrompt = $@"
        You are an expert English teacher for IT professionals (Level A2-B1).
        Your task is to evaluate the user's English translation of a Vietnamese sentence.
        
        Vietnamese sentence: ""{vietnameseText}""
        User's translation: ""{userTranslation}""
        
        Evaluate the translation based on gammar, vocavulary, and naturalness in an IT/working context.
        Respond ONLY with a valid JSON object using the following structure. Do not include markdown code blocks or any other text:
        {{
            ""is_correct"": boolean,
            ""corrected_sentence"": ""The most accurate English translation"",
            ""mistake_type"": ""Grammar""|""Vocabulary""|""Spelling""|""Punctuation|""None"",
            ""mistake_explanation"": ""Brief explaintion in Vietnamese about what was wrong and how to fix it. If correct, say 'Câu dịch rất tốt!'"",
            ""natural_alternatives"": [""Alternative way 1"", ""Alternative way 2""]
        }}
        ";  

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = systemPrompt
                        }
                    }
                }
            },
            generationConfig = new 
            {
                // Ép Gemini trả về json thuần túy
                response_mime_type = "application/json"
            }
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"); 

        var response = await _httpClient.PostAsync(url, jsonContent);
        if (!response.IsSuccessStatusCode)
        {
            // Xử lý lỗi nếu có
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Gemini API Error: {response.StatusCode} - {error}");
        }

        var responseString = await response.Content.ReadAsStringAsync();
        var geminiResponse = JsonSerializer.Deserialize<GeminiResponseDto>(responseString);

        if (geminiResponse?.Candidates != null && geminiResponse.Candidates.Count > 0)
        {
            var jsonText = geminiResponse.Candidates[0].Content.Parts[0].Text;
            // Parse chuỗi JSON từ AI thành object AiFeedbackDto
            var feedback = JsonSerializer.Deserialize<AiFeedbackDto>(jsonText);
            return feedback;
        }

        return null;
    }       

    public async Task<string> GetSessionRecommendationsAsync(List<string> userMistakes, bool isShort = false)
    {
        if (!GeminiRateLimiter.CheckAndIncrement(out var limitError))
        {
            throw new GeminiRateLimitException(limitError ?? "Hạn mức API Gemini đã vượt giới hạn.");
        }

        string apiKey = _config["GeminiApi:ApiKey"] ?? throw new Exception("Missing Gemini API Key");
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.1-flash-lite:generateContent?key={apiKey}";

        // Nối các lỗi sai thành 1 chuỗi
        string mistakesText = string.Join("\n", userMistakes.Select((m, i) => $"{i+1}. {m}"));
        
        string lengthConstraint = isShort 
            ? "Write very briefly and concisely, focusing ONLY on the most critical recurring issues. Use maximum 3 short bullet points per section, direct and action-oriented. Avoid long explanations and preamble."
            : "Write a detailed and comprehensive summary.";

        string systemPrompt = $@"    
            You are an expert English tutor for an IT professional (Level A2-B1).
            The student has completed translation practice.
            Below is a list of mistakes and feedback from their session:

            {mistakesText}

            Please analyze these mistakes and provide a summary in Vietnamese.
            {lengthConstraint}
            Your response must be structured clearly using Markdown with exactly the following 2 sections:
            ### Điểm còn yếu
            [Phân tích các điểm còn yếu chính]

            ### Cách khắc phục
            [Đề xuất giải pháp và các bước khắc phục cụ thể, bài học hoặc hướng luyện tập tiếp theo]
        ";

        var requestBody = new
        {
            contents = new[]
            {
                new 
                {
                    parts = new[]
                    {
                        new
                        {
                            text = systemPrompt
                        }
                    }
                }
            }
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, jsonContent);

        if (!response.IsSuccessStatusCode) 
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Gemini API Error: {response.StatusCode} - {error}");
        }

        var responseString = await response.Content.ReadAsStringAsync();
        var geminiResponse = JsonSerializer.Deserialize<GeminiResponseDto>(responseString);

        if (geminiResponse?.Candidates != null && geminiResponse.Candidates.Count > 0)
        {
            // Trả về chuỗi Markdown trực tiếp
            return geminiResponse.Candidates[0].Content.Parts[0].Text;
        }

        return "Không thể tạo đề xuất lúc này.";
    } 
}