using System.Text.Json.Serialization;

namespace LanguageLearningApp.DTOs;

public class AiFeedbackDto
{
    [JsonPropertyName("is_correct")]
    public bool IsCorrect { get; set; }

    [JsonPropertyName("corrected_sentence")]
    public string CorrectedSentence { get; set; } = string.Empty;

    [JsonPropertyName("mistake_type")]
    public string MistakeType { get; set; } = string.Empty; // VD: "Grammar", "Vocabulary", "Spelling", "Punctuation", "Style"

    [JsonPropertyName("mistake_explanation")]
    public string MistakeExplanation { get; set; } = string.Empty;

    [JsonPropertyName("natural_alternatives")]
    public List<string> NaturalAlternatives { get; set; } = new();
}