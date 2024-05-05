using Autofac;
using StackOverflow.Application.Contracts.Features;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.TagAggregateDTOs;

namespace StackOverflow.Web.Areas.Discussion.Models.TagModels;

public class TagListModel
{
    private ITagManagementService _tagManagementService;
    public IList<TagDto> Tags { get; set; }
    public string TagName { get; set; }
    public IList<QuestionDto> Questions { get; set; }

    public TagListModel() {}

    public TagListModel(ITagManagementService tagManagementService)
    {
        _tagManagementService = tagManagementService;
    }

    public void Resolve(ILifetimeScope scope)
    {
        _tagManagementService = scope.Resolve<ITagManagementService>();
    }

    public async Task GetAllTagsASync()
    {
        Tags = await _tagManagementService.GetAllTagsAsync();
    }
    
    public async Task GetQuestionByTagIdAsync(Guid tagId)
    {
        Questions = await _tagManagementService.GetQuestionsByTagIdAsync(tagId);
    }
}