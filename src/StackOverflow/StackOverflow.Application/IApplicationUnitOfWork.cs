using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;
using StackOverflow.Domain;
using StackOverflow.Domain.Repositories;

namespace StackOverflow.Application
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        IMemberRepository MemberRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        ITagRepository TagRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IAnswerRepository AnswerRepository { get; }
        Task<(IList<QuestionRetrievalDto> Questions, int total)> GetQuestionsAsync(int pageNumber, int pageSize, string? sortOption = null);
    }
}
