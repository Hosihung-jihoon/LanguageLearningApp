using System.ComponentModel.DataAnnotations;

namespace LanguageLearningApp.Models
{
    public class Topic
    {
        public int Id { get; set; } 
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation property: 1 topic có nhiều Sentences
        public List<Sentence> Sentences { get; set; } = new();
    }
}