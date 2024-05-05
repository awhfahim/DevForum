using StackOverflow.Domain.Entities;

namespace StackOverflow.Domain.Repositories;

public interface IQuestionRepository : IRepositoryBase<Question, Guid>
{
    Task<Question> GetQuestionWithCommentAsync(Guid id);
    Task<Question> GetByIdWithRelatedEntityAsync(Guid questionId);
    Task<Question> GetQuestionByIdWithTags(Guid questionId);
    Task<Question> GetQuestionByIdWithVotes(Guid id, Guid memberId);
    Task<IEnumerable<(string Title, string Body, Guid Id)>> GetQuestionsAsync(Guid userId);
    Task<IList<Question>> GetQuestionsByTagIdAsync(Guid tagId);
}