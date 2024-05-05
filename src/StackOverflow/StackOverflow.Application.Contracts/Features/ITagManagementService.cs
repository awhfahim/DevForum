using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.TagAggregateDTOs;

namespace StackOverflow.Application.Contracts.Features;

public interface ITagManagementService
{
    Task<IList<TagDto>> GetAllTagsAsync();
    Task<IList<QuestionDto>> GetQuestionsByTagIdAsync(Guid tagId);
}