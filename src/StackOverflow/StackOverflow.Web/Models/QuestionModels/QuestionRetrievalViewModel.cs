using Autofac;
using MapsterMapper;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.AnswerAggregateDTOs;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;
using StackOverflow.Application.Contracts.Utilities;

namespace StackOverflow.Web.Models.QuestionModels;

public class QuestionRetrievalViewModel
{
    private IQuestionManagementService _questionManagementService;
    private IMapper _mapper;
    private IDateTimeProvider _dateTimeProvider;
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; } 
    public int TotalVotes { get; set; }
    public int TotalAnswers { get; set; } 
    public List<QuestionCommentDto> QuestionComments { get; set; }
    public List<QuestionTagDto> QuestionTags { get; set; }
    public IEnumerable<CreateAnswerDto> Answers { get; set; }
    public int TotalQuestions { get; set; }

    public QuestionRetrievalViewModel()
    {
        QuestionComments = new List<QuestionCommentDto>();
        QuestionTags = new List<QuestionTagDto>();
        Answers = new List<CreateAnswerDto>();
    }
    public QuestionRetrievalViewModel(IQuestionManagementService questionManagementService, IMapper mapper, IDateTimeProvider dateTimeProvider)
    {
        _questionManagementService = questionManagementService;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
    }

    internal async Task<object> GetQuestionsAsync(int pageNumber, int pageSize, string sortOption)
    {
        var result = await _questionManagementService.GetAllQuestionAsync(pageNumber, pageSize, sortOption);
        TotalQuestions = result.total;
        return new
        {
            Questions = (from question in result.Questions
                select new
                {
                    question.Id,
                    question.Title,
                    question.Body,
                    question.CreatedAt,
                    question.TotalVotes,
                    question.TotalAnswers,
                }).ToList(),
        };
    }

    public async Task GetQuestionAsync(Guid id)
    {
        var questionDto = await _questionManagementService.GetQuestionByIdAsync(id);
        _mapper.Map(questionDto, this);
        Answers = await _questionManagementService.GetAnswersAsync(id);
    }

    internal void Resolve(ILifetimeScope scope)
    {
        _questionManagementService = scope.Resolve<IQuestionManagementService>();
        _mapper = scope.Resolve<IMapper>();
        _dateTimeProvider = scope.Resolve<IDateTimeProvider>();
    }

    public async Task EditQuestionAsync()
    {
        var questionDto = _mapper.Map<QuestionDto>(this);
        questionDto.UpdatedAt = _dateTimeProvider.GetUtcNow();
        questionDto.CreatedAt = _dateTimeProvider.GetUtcNow();
        await _questionManagementService.EditQuestionAsync(questionDto);
    }
}