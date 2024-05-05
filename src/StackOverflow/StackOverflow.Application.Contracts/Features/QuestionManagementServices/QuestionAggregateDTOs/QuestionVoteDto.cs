namespace StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;

public record QuestionVoteDto(Guid Id, Guid ApplicationUserId, VoteTypeDto VoteType);

public enum VoteTypeDto
{
    UpVote = 1, DownVote = -1
}