using Microsoft.AspNetCore.Identity;

namespace QABoard.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Question> Questions { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }
}