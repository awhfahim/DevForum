using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StackOverflow.Infrastructure.Membership;
using StackOverflow.Web.Areas.Discussion.Models.UserModels;

namespace StackOverflow.Web.Areas.Discussion.Controllers;

[Authorize]
[Area("Discussion")]
public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILifetimeScope _scope;
    public UserController(ILogger<UserController> logger, UserManager<ApplicationUser> userManager, ILifetimeScope scope)
    {
        _logger = logger;
        _userManager = userManager;
        _scope = scope;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Questions()
    {
        var userId = Guid.Parse(_userManager.GetUserId(User)!);
        var model = new QuestionListModel();
        model.Resolve(_scope);
        await model.GetQuestionsAsync(userId);
        _logger.Log(LogLevel.Information, "User questions retrieved successfully");
        return PartialView(model);
    }
}