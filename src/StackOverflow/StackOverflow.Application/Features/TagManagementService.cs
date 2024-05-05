using MapsterMapper;
using StackOverflow.Application.Contracts.Features;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.TagAggregateDTOs;

namespace StackOverflow.Application.Features;

public class TagManagementService : ITagManagementService
{
    private readonly IApplicationUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TagManagementService(IApplicationUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<TagDto>> GetAllTagsAsync()
    {
        var tags = await _unitOfWork.TagRepository.GetAllAsync();
        return _mapper.Map<IList<TagDto>>(tags);
    }

    public async Task<IList<QuestionDto>> GetQuestionsByTagIdAsync(Guid tagId)
    {
        var questions = await _unitOfWork.QuestionRepository.GetQuestionsByTagIdAsync(tagId);
        return _mapper.Map<IList<QuestionDto>>(questions);
    }
}