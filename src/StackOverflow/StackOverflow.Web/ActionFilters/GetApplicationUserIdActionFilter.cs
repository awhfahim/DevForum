using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using StackOverflow.Infrastructure.Membership;
using StackOverflow.Web.Areas.Discussion.Models;
using StackOverflow.Web.Models.QuestionModels;

namespace StackOverflow.Web.ActionFilters;

public class GetApplicationUserIdActionFilter : IActionFilter
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetApplicationUserIdActionFilter(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue("question", out var model)
            && model is AskViewModel askViewModel)
        {
            var userId = _userManager.GetUserId(context.HttpContext.User);
            askViewModel.ApplicationUserId = Guid.Parse(userId!);
        } 
        else if (context.ActionArguments.TryGetValue("model", out var model2)
                 && model2 is AnswerViewModel answerViewModel)
        {
            var userId = _userManager.GetUserId(context.HttpContext.User);
            answerViewModel.ApplicationUserId = Guid.Parse(userId!);
        }
        else if (context.ActionArguments.TryGetValue("model", out var model3)
                 && model3 is QuestionVoteViewModel questionVoteViewModel)
        {
            var userId = _userManager.GetUserId(context.HttpContext.User);
            questionVoteViewModel.ApplicationUserId = Guid.Parse(userId!);
        }
        else if (context.ActionArguments.TryGetValue("model", out var model4)
                 && model3 is QuestionCommentViewModel questionCommentViewModel)
        {
            var userId = _userManager.GetUserId(context.HttpContext.User);
            questionCommentViewModel.ApplicationUserId = Guid.Parse(userId!);
        }
        
       
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Do nothing
    }
}