using Autofac;
using MapsterMapper;
using StackOverflow.Application.Contracts;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;
using StackOverflow.Application.Contracts.Utilities;

namespace StackOverflow.Web.Models.QuestionModels;

public class AskViewModel
{
    private IQuestionManagementService _questionManagementService;
    private IDateTimeProvider _dateTimeProvider;
    private IMapper _mapper;
    public Guid ApplicationUserId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public List<string> Tags { get; set; }
    public DateTime CreatedAt;
    public AskViewModel() { }
    public AskViewModel(IQuestionManagementService questionManagementService, IDateTimeProvider dateTimeProvider,
        IMapper mapper)
    {
        _questionManagementService = questionManagementService;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task CreateQuestionAsync()
    {
        CreatedAt = _dateTimeProvider.GetUtcNow();
        var createQuestionDto = _mapper.Map<CreateQuestionDto>(this);
        await _questionManagementService.CreateQuestionAsync(createQuestionDto);
    }

    public void Resolve(ILifetimeScope scope)
    {
        _questionManagementService = scope.Resolve<IQuestionManagementService>();
        _dateTimeProvider = scope.Resolve<IDateTimeProvider>();
        _mapper = scope.Resolve<IMapper>();
    }
}