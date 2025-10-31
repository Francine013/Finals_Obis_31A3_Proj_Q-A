using QABoard.Infrastructure.Entities;

namespace QABoard.Services.Interfaces
{
    public interface IQuestionService
    {
        List<Question> GetAll();
        Question GetById(int id);
        void Create(Question question);
        void Update(Question question);
        void Delete(int id);
    }
}