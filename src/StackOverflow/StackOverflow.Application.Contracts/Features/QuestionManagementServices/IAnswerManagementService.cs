using StackOverflow.Application.Contracts.Features.QuestionManagementServices.AnswerAggregateDTOs;

namespace StackOverflow.Application.Contracts.Features.QuestionManagementServices;

public interface IAnswerManagementService
{
    Task<CreateAnswerDto> GetAnswerByQuestionIdAsync(Guid id);
}