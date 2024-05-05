using Microsoft.EntityFrameworkCore;
using StackOverflow.Application;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;
using StackOverflow.Domain.Repositories;
using StackOverflow.Infrastructure.DbContexts;

namespace StackOverflow.Infrastructure.UnitOfWorks
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IMemberRepository MemberRepository { get; }
        public IQuestionRepository QuestionRepository { get; }
        public ITagRepository TagRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IAnswerRepository AnswerRepository { get; }

        public ApplicationUnitOfWork(IApplicationDbContext dbContext, IMemberRepository userRepository, 
             IQuestionRepository questionRepository, ITagRepository tagRepository, IAnswerRepository answerRepository, INotificationRepository notificationRepository) : base((DbContext)dbContext)
        {
            MemberRepository = userRepository;
            QuestionRepository = questionRepository;
            TagRepository = tagRepository;
            AnswerRepository = answerRepository;
            NotificationRepository = notificationRepository;
        }
        
        public async Task<(IList<QuestionRetrievalDto> Questions, int total)> 
            GetQuestionsAsync(int pageNumber, int pageSize, string? sortOption = null)
        {
            const string procedureName = "GetQuestionsWithPaginationAndSorting";
            var data = await AdoNetUtility.QueryWithStoredProcedureAsync<QuestionRetrievalDto>(procedureName,
                new Dictionary<string, object>()
                {
                    {"pageNumber", pageNumber},
                    {"pageSize", pageSize},
                    {"sortOption", sortOption},
                    
                },
                new Dictionary<string, Type>
                {
                    { "Total",  typeof(int)}
                });
            data.outValues.TryGetValue("Total", out var total);
            return (data.result, total is null ? 0 : (int)total);
        }
	}
}
