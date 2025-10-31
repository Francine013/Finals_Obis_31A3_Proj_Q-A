using QABoard.Infrastructure.Entities;

namespace QABoard.Services.Interfaces
{
    public interface IAnswerService
    {
        List<Answer> GetByQuestionId(int questionId);
        Answer GetById(int id);
        void Create(Answer answer);
        void Update(Answer answer);
        void Delete(int id);
    }
}