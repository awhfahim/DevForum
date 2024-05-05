using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverflow.Web.ActionFilters;
using StackOverflow.Web.Areas.Discussion.Models;

namespace StackOverflow.Web.Areas.Discussion.Controllers;

[Authorize]
[Area("Discussion")]
public class AnswerController : Controller
{
    private ILogger<AnswerController> _logger;
    private ILifetimeScope _scope;

    public AnswerController(ILifetimeScope scope, ILogger<AnswerController> logger)
    {
        _scope = scope;
        _logger = logger;
    }

    [HttpPost, ValidateAntiForgeryToken]
    [ServiceFilter(typeof(GetApplicationUserIdActionFilter))]
    public async Task<IActionResult> CreateAnswer(AnswerViewModel model)
    {
        if (ModelState.IsValid)
        {
            model.Resolve(_scope);
            await model.CreateAnswerAsync();
            _logger.LogInformation("Answer created successfully for question {QuestionId}", model.QuestionId);
            return RedirectToAction("Question", "Question", new { id = model.QuestionId });
        }
        _logger.LogError("Answer creation failed for question {QuestionId}", model.QuestionId);
        return RedirectToAction("Question", "Question", new { id = model.QuestionId });
    }
    
    public async Task<IActionResult> AddVote(AnswerVoteViewModel model)
    {
        model.Resolve(_scope);
        await model.AddVoteAsync();
        return RedirectToAction("Question", "Question", new { id = model.QuestionId });
    }
}