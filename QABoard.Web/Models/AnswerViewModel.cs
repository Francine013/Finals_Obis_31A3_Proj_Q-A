using System.ComponentModel.DataAnnotations;

namespace QABoard.Web.Models
{
    public class AnswerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Answer content is required")]
        public string Content { get; set; }

        public int QuestionId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int VoteCount { get; set; }
        public bool HasVoted { get; set; }
    }
}