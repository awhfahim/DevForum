using Autofac;
using MapsterMapper;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.AnswerAggregateDTOs;
using StackOverflow.Application.Contracts.Utilities;

namespace StackOverflow.Web.Areas.Discussion.Models;

public class AnswerViewModel
{
    private IQuestionManagementService _questionManagementService;
    private IDateTimeProvider _dateTimeProvider;
    private IMapper _mapper;

    public string Body { get; set; }
    public Guid QuestionId { get; set; }
    public Guid ApplicationUserId { get; set; }

    public AnswerViewModel()
    {
        
    }
    public AnswerViewModel(IQuestionManagementService questionManagementService, IDateTimeProvider dateTimeProvider,
        IMapper mapper)
    {
        _questionManagementService = questionManagementService;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task CreateAnswerAsync()
    {
        var answerDto = _mapper.Map<CreateAnswerDto>(this);
        answerDto.AnsweredAt = _dateTimeProvider.GetUtcNow();
        await _questionManagementService.CreateAnswerAsync(answerDto);
    }

    public void Resolve(ILifetimeScope scope)
    {
        _questionManagementService = scope.Resolve<IQuestionManagementService>();
        _dateTimeProvider = scope.Resolve<IDateTimeProvider>();
        _mapper = scope.Resolve<IMapper>();
    }
}