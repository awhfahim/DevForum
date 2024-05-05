namespace StackOverflow.Domain.Entities;

public class Notification : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public NotificationType Type { get; set; } // "Comment" or "Answer"
    public Guid EntityId { get; set; } // CommentId or AnswerId
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum NotificationType
{
    Comment,
    Answer
}