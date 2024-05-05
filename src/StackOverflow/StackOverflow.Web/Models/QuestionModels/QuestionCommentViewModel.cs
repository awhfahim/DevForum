using Autofac;
using Microsoft.AspNetCore.Identity;
using StackOverflow.Application.Contracts.Features.AccountManagementServices;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;
using StackOverflow.Application.Contracts.Utilities;
using StackOverflow.Infrastructure.Membership;

namespace StackOverflow.Web.Models.QuestionModels;

public class QuestionCommentViewModel
{
    private IQuestionManagementService _questionManagementService;
    private IDateTimeProvider _dateTimeProvider;
    private IMemberManagementService _memberManagementService;
    
    public Guid ApplicationUserId { get; set; }
    public Guid QuestionId { get; set; }
    public string Text { get; set; }
    public DateTime CreationTime { get; set; }
    public QuestionCommentViewModel()
    {
    }
    public QuestionCommentViewModel(IQuestionManagementService questionManagementService, 
        IMemberManagementService memberManagementService, UserManager<ApplicationUser> userManager)
    {
        _questionManagementService = questionManagementService;
        _memberManagementService = memberManagementService;
    }
    internal void Resolve(ILifetimeScope scope)
    {
        _questionManagementService = scope.Resolve<IQuestionManagementService>();
        _dateTimeProvider = scope.Resolve<IDateTimeProvider>();
        _memberManagementService = scope.Resolve<IMemberManagementService>();
    }

    public async Task AddCommentAsync()
    {
        await _questionManagementService.AddCommentAsync(new QuestionCommentDto
        (
            
            QuestionId,
            ApplicationUserId,
            Text,
            CreationTime = _dateTimeProvider.GetUtcNow()
        ));
    }

    public async Task<bool> CheckUserReputationAsync(Guid userId)
    {
        var currentReputation = await _memberManagementService.GetReputationAsync(userId);
        return currentReputation > 50;
    } 
}