using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;

namespace StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;

public record QuestionDto
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int TotalVotes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public uint AnswersCount { get; set; }
    public List<QuestionCommentDto> QuestionComments { get; set; }
    public List<QuestionTagDto> QuestionTags { get; set; }
}