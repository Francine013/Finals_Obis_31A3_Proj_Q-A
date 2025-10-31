using System.ComponentModel.DataAnnotations;

namespace QABoard.Infrastructure.Entities
{
    public class Answer
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public ICollection<Vote> Votes { get; set; }
    }
}