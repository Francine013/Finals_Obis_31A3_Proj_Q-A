using Microsoft.EntityFrameworkCore;
using QABoard.Infrastructure.Data;
using QABoard.Infrastructure.Entities;
using QABoard.Services.Interfaces;

namespace QABoard.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly AppDbContext _context;

        public AnswerService(AppDbContext context)
        {
            _context = context;
        }

        public List<Answer> GetByQuestionId(int questionId)
        {
            return _context.Answers
                .Include(a => a.User)
                .Include(a => a.Votes)
                .Where(a => a.QuestionId == questionId)
                .OrderByDescending(a => a.Votes.Count)
                .ToList();
        }

        public Answer GetById(int id)
        {
            return _context.Answers
                .Include(a => a.User)
                .FirstOrDefault(a => a.Id == id);
        }

        public void Create(Answer answer)
        {
            _context.Answers.Add(answer);
            _context.SaveChanges();
        }

        public void Update(Answer answer)
        {
            _context.Answers.Update(answer);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var answer = _context.Answers.Find(id);
            if (answer != null)
            {
                _context.Answers.Remove(answer);
                _context.SaveChanges();
            }
        }
    }
}