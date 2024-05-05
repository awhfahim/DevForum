using MapsterMapper;
using Serilog;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.AnswerAggregateDTOs;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;
using StackOverflow.Application.Contracts.Utilities;
using StackOverflow.Domain.Entities;
using StackOverflow.Domain.Exceptions;
using VoteType = StackOverflow.Domain.Entities.VoteType;

namespace StackOverflow.Application.Features.QuestionManagementServices;

public class QuestionManagementService : IQuestionManagementService
{
    private readonly IApplicationUnitOfWork _unitOfWork;
    private readonly IGuidProvider _guidProvider;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public QuestionManagementService(IApplicationUnitOfWork unitOfWork, 
        IGuidProvider guidProvider, IMapper mapper, ILogger logger, IDateTimeProvider dateTimeProvider)
    {
        _unitOfWork = unitOfWork;
        _guidProvider = guidProvider;
        _mapper = mapper;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }
    
    //Tested
    public async Task CreateQuestionAsync(CreateQuestionDto createQuestionDto)
    {
        try
        {
            if (createQuestionDto is null)
                throw new ArgumentNullException(nameof(createQuestionDto));
            
            var question = new Question(
                _guidProvider.GetGuid(),
                createQuestionDto.ApplicationUserId,
                createQuestionDto.Title,
                createQuestionDto.Body,
                createQuestionDto.CreatedAt
            );
            await SetTagsAsync(createQuestionDto.Tags, question);
            await _unitOfWork.QuestionRepository.AddAsync(question);
            await _unitOfWork.SaveAsync();
        }

        catch (ArgumentNullException ex)
        {
            _logger.Error("CreateQuestionAsync failed {@ex}", ex);
             throw;
        }
        catch (NotFoundException ex)
        {
            _logger.Error("CreateQuestionAsync failed {@ex}", ex);
            throw;
        }
        catch (Exception e)
        {
            _logger.Error("CreateQuestionAsync failed {@e}", e);
            throw;
        }
    }
    
    //Tested
    public async Task<(IList<QuestionRetrievalDto> Questions, int total)>
        GetAllQuestionAsync(int pageNumber, int pageSize, string sortOption)
       => await _unitOfWork.GetQuestionsAsync(pageNumber, pageSize, sortOption);
    
    //Tested
    public async Task<QuestionDto?> GetQuestionByIdAsync(Guid id)
    {
        var data = await _unitOfWork.QuestionRepository.GetQuestionWithCommentAsync(id);
        var questionTags = new List<QuestionTagDto>();

        if (data is null) return null;
        foreach (var questionTag in data.QuestionTags)
        {
            var tag = await _unitOfWork.TagRepository.GetTagByIdAsync(questionTag.TagId);
            questionTags.Add(new QuestionTagDto(tag.Name));
        }
        var result = _mapper.Map<QuestionDto>(data!);
        result.QuestionTags = questionTags;
        result.TotalVotes = data.VotesCount;
        return result;

    }
    
    //Tested
    public async Task AddCommentAsync(QuestionCommentDto questionCommentDto)
    {   
        var question = await _unitOfWork.QuestionRepository.GetByIdWithRelatedEntityAsync(questionCommentDto.QuestionId);
        if(question is null)
            throw new NotFoundException(nameof(Question), questionCommentDto.QuestionId.ToString());
        
        question.AddQuestionComment(new QuestionComment(
            questionCommentDto.MemberId,
            questionCommentDto.QuestionId,
            questionCommentDto.Text,
            questionCommentDto.CreationTime));
        
        await _unitOfWork.NotificationRepository.AddAsync(new Notification()
        {
            CreatedAt = _dateTimeProvider.GetUtcNow(),
            EntityId = questionCommentDto.QuestionId,
            IsRead = false,
            MemberId = question.MemberId,
            Type = NotificationType.Comment
        });
        await _unitOfWork.SaveAsync();
    }
    
    //Tested
    public async Task CreateAnswerAsync(CreateAnswerDto answerDto)
    {
        if(answerDto is null)
            throw new ArgumentNullException(nameof(answerDto));
        
        var answer = _mapper.Map<Answer>(answerDto);
        answer.MemberId = answerDto.AppliationUserId;
        answer.AnswerStatus = AnswerStatus.Accepted;
        var member = await _unitOfWork.MemberRepository.GetByIdAsync(answer.MemberId);
        var questioner = await _unitOfWork.QuestionRepository.GetByIdAsync(answerDto.QuestionId);
        
        member.Reputation += 10;
        
        await _unitOfWork.NotificationRepository.AddAsync(new Notification()
        {
            CreatedAt = _dateTimeProvider.GetUtcNow(),
            EntityId = answerDto.QuestionId,
            IsRead = false,
            MemberId = questioner.MemberId,
            Type = NotificationType.Answer
        });
        
        await _unitOfWork.AnswerRepository.AddAsync(answer);
        await _unitOfWork.SaveAsync();
    }
    
    //Tested
    public async Task<IEnumerable<CreateAnswerDto>> GetAnswersAsync(Guid id)
    {
        var answers = await _unitOfWork.AnswerRepository.GetAnswersByQuestionIdAsync(id);
        var result = _mapper.Map<IEnumerable<CreateAnswerDto>>(answers);
        return result;
    }
    
    //Tested
    public async Task EditQuestionAsync(QuestionDto questionDto)
    {
        var question = await _unitOfWork.QuestionRepository.GetQuestionByIdWithTags(questionDto.Id);
        if (question is null)
            throw new NotFoundException(nameof(Question), questionDto.Id.ToString());
        
        _mapper.Map(questionDto, question);
        var tagNames = (from tagDto in questionDto.QuestionTags select tagDto.TagName).ToList();
        await SetTagsAsync(tagNames, question);
        await _unitOfWork.SaveAsync();
    }

    public async Task AddVoteAsync(QuestionVoteDto questionVoteDto)
    {
        var memberId = questionVoteDto.ApplicationUserId;
        
        var question = await _unitOfWork.QuestionRepository.GetQuestionByIdWithVotes(questionVoteDto.Id, memberId);

        var member = await _unitOfWork.MemberRepository.GetByIdAsync(question.MemberId);
        
        member.Reputation += questionVoteDto.VoteType switch
        {
            VoteTypeDto.UpVote => 10,
            VoteTypeDto.DownVote when member.Reputation > 0 => -2,
            _ => 0,
        };
        
        if (question is null)
            throw new NotFoundException(nameof(Question), questionVoteDto.Id.ToString());
        
        question.RemoveQuestionVote(memberId);
        question.AddQuestionVote(memberId,(VoteType)questionVoteDto.VoteType);
        question.VotesCount += questionVoteDto.VoteType == VoteTypeDto.UpVote
            ? 1
            : -1;
        await _unitOfWork.SaveAsync();
    }

    public async Task<bool> CheckIfVoteExists(Guid id, Guid applicationUserId, VoteTypeDto voteType)
    {
        var memberId = applicationUserId;
        var isVoteExists = await _unitOfWork.QuestionRepository
            .GetCountAsync(x => x.QuestionVotes.Any(y => y.MemberId == memberId && y.QuestionId == id));

        if (isVoteExists <= 0) return false;
        {
            var isSameVoteExist = await _unitOfWork.QuestionRepository
                .GetCountAsync(x => x.QuestionVotes.Any(y => y.MemberId == memberId && y.QuestionId == id && y.VoteType == (VoteType)voteType));

            return isSameVoteExist != 0;
        }

    }

    public async Task<IList<QuestionDto>> GetQuestionsAsync(Guid userId)
    {
        var data = await _unitOfWork.QuestionRepository.GetQuestionsAsync(userId);
        var result = _mapper.Map<IList<QuestionDto>>(data);
        return result;
    }

    private async Task SetTagsAsync(List<string> tags, Question question)
    {
        foreach (var tag in tags)
        {
            var tagEntity = await _unitOfWork.TagRepository.GetTagByNameAsync(tag);
            if (tagEntity is null)
            {
                tagEntity = new Tag(_guidProvider.GetGuid(), tag);
                await _unitOfWork.TagRepository.AddAsync(tagEntity);
                await _unitOfWork.SaveAsync();
                question.AddQuestionTag(tagEntity.Id);
            }
            else
            {
                var result = question.QuestionTags.FirstOrDefault(x => x.TagId == tagEntity.Id);
                if (result is null)
                    question.AddQuestionTag(tagEntity.Id);
            }
        }
    }
}
