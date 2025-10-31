using QABoard.Infrastructure.Data;
using QABoard.Infrastructure.Entities;
using QABoard.Services.Interfaces;

namespace QABoard.Services.Implementations
{
    public class VoteService : IVoteService
    {
        private readonly AppDbContext _context;

        public VoteService(AppDbContext context)
        {
            _context = context;
        }

        public bool Upvote(int answerId, string userId)
        {
            var existingVote = _context.Votes
                .FirstOrDefault(v => v.AnswerId == answerId && v.UserId == userId);

            if (existingVote != null)
            {
                return false;
            }

            var vote = new Vote
            {
                AnswerId = answerId,
                UserId = userId
            };

            _context.Votes.Add(vote);
            _context.SaveChanges();
            return true;
        }

        public void RemoveVote(int answerId, string userId)
        {
            var vote = _context.Votes
                .FirstOrDefault(v => v.AnswerId == answerId && v.UserId == userId);

            if (vote != null)
            {
                _context.Votes.Remove(vote);
                _context.SaveChanges();
            }
        }

        public int GetVoteCount(int answerId)
        {
            return _context.Votes.Count(v => v.AnswerId == answerId);
        }

        public bool HasUserVoted(int answerId, string userId)
        {
            return _context.Votes.Any(v => v.AnswerId == answerId && v.UserId == userId);
        }
    }
}