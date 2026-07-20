using System.Text.Json.Serialization;

namespace LanguageLearningApp.DTOs;

public class GeminiResponseDto
{
    [JsonPropertyName("candidates")]
    public List<Candidate> Candidates { get; set; } = new();
}

public class Candidate
{
    [JsonPropertyName("content")]
    public Content Content { get; set; } = new();
}

public class Content
{
    [JsonPropertyName("parts")]
    public List<Part> Parts { get; set; } = new();
}

public class Part
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}