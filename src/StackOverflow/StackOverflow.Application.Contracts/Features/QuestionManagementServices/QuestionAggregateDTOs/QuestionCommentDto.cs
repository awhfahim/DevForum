namespace StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;

public record QuestionCommentDto(Guid QuestionId, Guid MemberId, string Text, DateTime CreationTime);