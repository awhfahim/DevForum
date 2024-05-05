namespace StackOverflow.Application.Contracts.Features.NotificationDTOs;

public record NotificationDto(Guid Id, NotificationType Type, Guid EntityId, bool IsRead, DateTime CreatedAt);
public enum NotificationType
{
    Comment,
    Answer
}