using System.ComponentModel.DataAnnotations;

namespace LanguageLearningApp.Models
{
    public class Sentence
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        [Required, MaxLength(500)]
        public string VietnameseText { get; set; } = string.Empty;
        [Required, MaxLength(500)]
        public string EnglishText { get; set; } = string.Empty;
        
        public string? Phonetic { get; set; }
        
        [Required, MaxLength(50)]
        public string Type { get; set; } = "Single";
        
        public int? ConversationId { get; set; }

        // Navigation property
        public Topic? Topic { get; set; }
        public List<UserAttempt> UserAttempts { get; set; } = new();
    }
}