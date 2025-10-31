namespace QABoard.Infrastructure.Entities
{
    public class Vote
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int AnswerId { get; set; }
        public Answer Answer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}