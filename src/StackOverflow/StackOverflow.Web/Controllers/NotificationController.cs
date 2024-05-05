using Autofac;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StackOverflow.Infrastructure.Membership;
using StackOverflow.Web.Models.NotificationModels;

namespace StackOverflow.Web.Controllers;

public class NotificationController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILifetimeScope _scope;

    public NotificationController(UserManager<ApplicationUser> userManager, ILifetimeScope scope)
    {
        _userManager = userManager;
        _scope = scope;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var model = new NotificationListModel();
        var user = _userManager.GetUserId(User);
        model.Resolve(_scope);
        var notifications = await model.GetNotificationsAsync(Guid.Parse(user!));
        
        return Json(notifications);
    }
}