namespace QABoard.Services.Interfaces
{
    public interface IVoteService
    {
        bool Upvote(int answerId, string userId);
        void RemoveVote(int answerId, string userId);
        int GetVoteCount(int answerId);
        bool HasUserVoted(int answerId, string userId);
    }
}