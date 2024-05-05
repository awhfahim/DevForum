namespace StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;

public record CreateQuestionDto(Guid ApplicationUserId, string Title, string Body, List<string> Tags, DateTime CreatedAt);