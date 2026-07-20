using LanguageLearningApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LanguageLearningApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Sentence> Sentences { get; set; }
        public DbSet<UserAttempt> UserAttempts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình quan hệ và ràng buộc
            modelBuilder.Entity<Sentence>()
                .HasOne(s => s.Topic)
                .WithMany(t => t.Sentences)
                .HasForeignKey(s => s.TopicId);

            modelBuilder.Entity<UserAttempt>()
                .HasOne(a => a.Sentence)
                .WithMany(s => s.UserAttempts)
                .HasForeignKey(a => a.SentenceId);
        }
    }
}                                               