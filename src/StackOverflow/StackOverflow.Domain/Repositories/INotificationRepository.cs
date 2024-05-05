using StackOverflow.Domain.Entities;

namespace StackOverflow.Domain.Repositories;

public interface INotificationRepository : IRepositoryBase<Notification, Guid>
{
    Task<IList<Notification>> GetUnreadNotifications(Guid userId);
}