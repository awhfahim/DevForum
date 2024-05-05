using StackOverflow.Domain.Entities;

namespace StackOverflow.Domain.Repositories;

public interface IAnswerRepository : IRepositoryBase<Answer, Guid>
{
    Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid id);
}