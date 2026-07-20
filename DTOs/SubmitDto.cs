namespace LanguageLearningApp.DTOs;

public class SubmitDto
{
    public int SentenceId { get; set; }
    public string UserTranslation { get; set; } = string.Empty;
}