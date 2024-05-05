namespace StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;

public record QuestionRetrievalDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; } 
    public int TotalVotes { get; set; }
    public int TotalAnswers { get; set; } 
}