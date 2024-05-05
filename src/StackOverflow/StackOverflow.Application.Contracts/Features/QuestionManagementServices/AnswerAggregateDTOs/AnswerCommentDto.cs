namespace StackOverflow.Application.Contracts.Features.QuestionManagementServices.AnswerAggregateDTOs;

public record AnswerCommentDto(Guid AnswerId, string Text, DateTime CreationTime);