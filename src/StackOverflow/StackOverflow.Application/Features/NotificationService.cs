using MapsterMapper;
using StackOverflow.Application.Contracts.Features;
using StackOverflow.Application.Contracts.Features.NotificationDTOs;

namespace StackOverflow.Application.Features;

public class NotificationService : INotificationService
{
    private readonly IApplicationUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public NotificationService(IApplicationUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task MarkAsRead(Guid notificationId)
    {
        var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(notificationId);
        notification.IsRead = true;
        await _unitOfWork.SaveAsync();
    }

    public async Task<List<NotificationDto>> GetUnreadNotifications(Guid userId)
    {
        var notifications = await _unitOfWork.NotificationRepository.GetUnreadNotifications(userId);
        return notifications is null ? null : _mapper.Map<List<NotificationDto>>(notifications);
    }
}