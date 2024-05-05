using StackOverflow.Application.Contracts.Features.NotificationDTOs;

namespace StackOverflow.Application.Contracts.Features;

public interface INotificationService
{
    Task MarkAsRead(Guid notificationId);
    Task<List<NotificationDto>> GetUnreadNotifications(Guid userId);
}