using Autofac;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using StackOverflow.Application.Contracts.Features.AccountManagementServices;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;
using StackOverflow.Domain.consts;
using StackOverflow.Domain.Repositories;
using StackOverflow.Infrastructure.Membership;
using VoteType = StackOverflow.Domain.Entities.VoteType;

namespace StackOverflow.Web.Models.QuestionModels;

public class QuestionVoteViewModel
{
    private IQuestionManagementService _questionManagementService;
    private IMapper _mapper;
    private UserManager<ApplicationUser> _userManager;
    private IMemberManagementService _memberManagementService;

    public QuestionVoteViewModel()
    {
    }
    public QuestionVoteViewModel(IQuestionManagementService questionManagementService, IMapper mapper, 
        UserManager<ApplicationUser> userManager, IMemberManagementService memberManagementService)
    {
        _questionManagementService = questionManagementService;
        _mapper = mapper;
        _userManager = userManager;
        _memberManagementService = memberManagementService;
    }

    public Guid Id { get; set; }
    public VoteType VoteType { get; set; }
    public Guid ApplicationUserId { get; set; }

    public void Resolve(ILifetimeScope scope)
    {
        _questionManagementService = scope.Resolve<IQuestionManagementService>();
        _mapper = scope.Resolve<IMapper>();
        _userManager = scope.Resolve<UserManager<ApplicationUser>>();
        _memberManagementService = scope.Resolve<IMemberManagementService>();
    }
    public async Task<string> AddVoteAsync()
    {
        try
        {
            var user = await GetUserAsync();
            var result =  await CheckUserReputation(user);

            if (result != Messages.VoteAllowed)
            {
                return result;
            }

            if (await CheckIfVoteExists())
            {
                return Messages.VoteAlreadyExists;
            }

            await AddVote();
            return Messages.VoteAddedSuccessfully;
        }
        catch (Exception ex)
        {
            // Log the exception and return an error message
            //_logger.LogError(ex, "Error adding vote");
            return Messages.ErrorAddingVote;
        }
    }
    
    private async Task AddVote()
    {
        var questionVoteDto = _mapper.Map<QuestionVoteDto>(this);
        await _questionManagementService.AddVoteAsync(questionVoteDto);
    }
    
    private async Task<bool> CheckIfVoteExists() 
        => await _questionManagementService.CheckIfVoteExists(Id, ApplicationUserId, (VoteTypeDto)VoteType);
    
    private async Task<ApplicationUser> GetUserAsync() 
        => await _userManager.FindByIdAsync(ApplicationUserId.ToString()) ?? throw new NullReferenceException();

    private async Task<string> CheckUserReputation(ApplicationUser user)
    {
        var currentReputation = await _memberManagementService.GetReputationAsync(user.Id);
        var result = VoteType switch
        {
            VoteType.UpVote when currentReputation < 15 => Messages.UpvoteNotAllowed,
            VoteType.DownVote when currentReputation < 125 => Messages.DownvoteNotAllowed,
            _ => Messages.VoteAllowed
        };
        return result;
    } 
}