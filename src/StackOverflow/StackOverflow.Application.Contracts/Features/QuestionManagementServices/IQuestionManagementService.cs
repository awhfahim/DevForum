using StackOverflow.Application.Contracts.Features.QuestionManagementServices.AnswerAggregateDTOs;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;

namespace StackOverflow.Application.Contracts.Features.QuestionManagementServices;

public interface IQuestionManagementService
{
    Task CreateQuestionAsync(CreateQuestionDto createQuestionDto);
    Task<(IList<QuestionRetrievalDto> Questions, int total)> GetAllQuestionAsync(int pageNumber, int pageSize, string sortOption);
    Task<QuestionDto?> GetQuestionByIdAsync(Guid id);
    Task AddCommentAsync(QuestionCommentDto questionCommentDto);
    Task CreateAnswerAsync(CreateAnswerDto answerDto);
    Task<IEnumerable<CreateAnswerDto>> GetAnswersAsync(Guid id);
    Task EditQuestionAsync(QuestionDto questionDto);
    Task AddVoteAsync(QuestionVoteDto questionVoteDto);
    Task<bool> CheckIfVoteExists(Guid id, Guid applicationUserId, VoteTypeDto voteType);
    Task<IList<QuestionDto>> GetQuestionsAsync(Guid userId);
}       