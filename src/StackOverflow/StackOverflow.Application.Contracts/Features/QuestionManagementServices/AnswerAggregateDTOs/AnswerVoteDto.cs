namespace StackOverflow.Application.Contracts.Features.QuestionManagementServices.AnswerAggregateDTOs;

public record AnswerVoteDto(Guid AnswerId, int VoteType, DateTime VotedAt);