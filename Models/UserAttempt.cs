namespace LanguageLearningApp.Models;

public class UserAttempt
{
    public int Id { get; set; }
    public int SentenceId { get; set; }
    public string UserTranslation { get; set; } = string.Empty; // Câu user gõ 
    public string AiFeedback { get; set; } = string.Empty; // Lưu feedback AI (dang json string)
    public string MistakeType { get; set; } = string.Empty; // VD: "Grammar", "Vocabulary", "Structure", "Naturalness", "All"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Sentence? Sentence { get; set; }
}