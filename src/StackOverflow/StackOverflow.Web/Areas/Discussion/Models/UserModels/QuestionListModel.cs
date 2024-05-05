using Autofac;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;

namespace StackOverflow.Web.Areas.Discussion.Models.UserModels;

public class QuestionListModel
{
    private   IQuestionManagementService _questionManagementService;

    public List<QuestionDto> Questions { get; set; }

    public QuestionListModel()
    {
        
    }
    public QuestionListModel(IQuestionManagementService questionManagementService)
    {
        _questionManagementService = questionManagementService;
    }

    public void Resolve(ILifetimeScope scope)
    {
        _questionManagementService = scope.Resolve<IQuestionManagementService>();
    }

    public async Task GetQuestionsAsync(Guid userId)
    {
        var result = await _questionManagementService.GetQuestionsAsync(userId);
        Questions = result.ToList();
    }
}