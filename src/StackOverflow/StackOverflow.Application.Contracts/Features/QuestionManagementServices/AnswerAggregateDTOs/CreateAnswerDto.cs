namespace StackOverflow.Application.Contracts.Features.QuestionManagementServices.AnswerAggregateDTOs;

public record CreateAnswerDto
{
    public Guid Id { get; set; }
    public Guid AppliationUserId { get; set; }
    public Guid QuestionId { get; set; }
    public string Body { get; set; }
    public bool IsAccepted { get; set; }
    public int Votes { get; set; }
    public DateTime AnsweredAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public IList<AnswerCommentDto> AnswerComments { get; set; }
    public IList<AnswerVoteDto> AnswerVotes { get; set; }
}