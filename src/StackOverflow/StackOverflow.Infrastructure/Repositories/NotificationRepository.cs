using Microsoft.EntityFrameworkCore;
using StackOverflow.Domain.Entities;
using StackOverflow.Domain.Repositories;
using StackOverflow.Infrastructure.DbContexts;

namespace StackOverflow.Infrastructure.Repositories;

public class NotificationRepository : Repository<Notification,Guid>, INotificationRepository
{
    public NotificationRepository(IApplicationDbContext context) : base((DbContext)context)
    {
    }
    public async Task<IList<Notification>> GetUnreadNotifications(Guid userId)
     => (await GetAsync(x => x, 
            x => x.MemberId == userId && x.IsRead == false)).ToList();
}