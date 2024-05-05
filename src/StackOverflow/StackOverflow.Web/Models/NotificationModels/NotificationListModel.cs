using Autofac;
using StackOverflow.Application.Contracts.Features;
using StackOverflow.Application.Contracts.Features.NotificationDTOs;

namespace StackOverflow.Web.Models.NotificationModels;

public class NotificationListModel
{
    private INotificationService _notificationService;

    public NotificationListModel()
    {
        
    }
    public NotificationListModel(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<IList<object>> GetNotificationsAsync(Guid applicalionUserId)
    {
        var notifications = await _notificationService.GetUnreadNotifications(applicalionUserId);
        var notificationList = new List<object>();
        foreach (var notification in notifications)
        {
            notificationList.Add(new
            {
                Text = $"You have a new {notification.Type} to your question",
                QuestionId = notification.EntityId
            });
        }

        return notificationList;
    }

    public void Resolve(ILifetimeScope scope)
    {
        _notificationService = scope.Resolve<INotificationService>();
    }
}