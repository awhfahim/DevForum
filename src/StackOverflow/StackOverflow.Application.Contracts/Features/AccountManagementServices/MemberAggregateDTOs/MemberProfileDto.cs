namespace StackOverflow.Application.Contracts.Features.AccountManagementServices.MemberAggregateDTOs;

public record MemberProfileDto
(
    string DisplayName, 
    string? FullName, 
    string? Location, 
    string? AboutMe, 
    string? WebsiteLink, 
    string? TwitterUsername, 
    string? GitHubUsername
);