using System.ComponentModel.DataAnnotations;

namespace QABoard.Infrastructure.Entities
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}