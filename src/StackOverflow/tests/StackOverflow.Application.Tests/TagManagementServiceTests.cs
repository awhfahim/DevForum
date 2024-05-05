using Autofac.Extras.Moq;
using MapsterMapper;
using Moq;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.TagAggregateDTOs;
using StackOverflow.Application.Features;
using StackOverflow.Domain.Entities;
using StackOverflow.Domain.Repositories;

namespace StackOverflow.Application.Tests;

public class TagManagementServiceTests
{
    private AutoMock _mock;
    private Mock<IApplicationUnitOfWork> _unitOfWork;
    private Mock<IMapper> _mapper;
    private Mock<ITagRepository> _tagRepository;
    private Mock<IQuestionRepository> _questionRepository;
    private TagManagementService _tagManagementService;
    
    [SetUp]
    public void Setup()
    {
        _unitOfWork = _mock.Mock<IApplicationUnitOfWork>();
        _tagRepository = _mock.Mock<ITagRepository>();
        _questionRepository= _mock.Mock<IQuestionRepository>();
        _mapper = _mock.Mock<IMapper>();
        _tagManagementService = _mock.Create<TagManagementService>();
    }
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _mock = AutoMock.GetLoose();
    }
    
    [TearDown]
    public void TearDown()
    {
        _unitOfWork.Reset();
        _mapper.Reset();
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _mock.Dispose();
    }

    [Test]
    public async Task GetAllTagsAsync_ShouldReturnAllTags()
    {
        // Arrange
        var tags = new List<Tag>()
        {
            new Tag(Guid.NewGuid(), "tag1"),
            new Tag(Guid.NewGuid(), "tag2"),
            new Tag(Guid.NewGuid(), "tag3")
        };
        var tagDtoList = new List<TagDto>()
        {
            new TagDto(Guid.NewGuid(), "tag1"),
            new TagDto(Guid.NewGuid(), "tag2"),
            new TagDto(Guid.NewGuid(), "tag3")  
        };
        
        _tagRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(tags).Verifiable();
        
        _unitOfWork.SetupGet(x => x.TagRepository)
            .Returns(_tagRepository.Object).Verifiable();
        
        _mapper.Setup(x => x.Map<IList<TagDto>>(It.IsAny<IList<Tag>>()))
            .Returns(tagDtoList).Verifiable();
        
        
        // Act
        var result = await _tagManagementService.GetAllTagsAsync();
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(tags.Count));
    }
    
    [Test]
    public async Task GetQuestionsByTagIdAsync_ShouldReturnQuestionsByTagId()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var questions = new List<Question>()
        {
            new Question(Guid.NewGuid(),Guid.NewGuid(), "Question1", "Description1", DateTime.Now),
            new Question(Guid.NewGuid(),Guid.NewGuid(), "Question2", "Description2", DateTime.Now),
            new Question(Guid.NewGuid(),Guid.NewGuid(), "Question3", "Description3", DateTime.Now)
        };
        var questionDtoList = new List<QuestionDto>()
        {
            new QuestionDto(),
            new QuestionDto(),
            new QuestionDto()
        };
        
        _unitOfWork.SetupGet(x => x.QuestionRepository)
            .Returns(_questionRepository.Object).Verifiable();
        
        _unitOfWork.Setup(x => x.QuestionRepository.GetQuestionsByTagIdAsync(tagId))
            .ReturnsAsync(questions).Verifiable();
        
        _mapper.Setup(x => x.Map<IList<QuestionDto>>(It.IsAny<IList<Question>>()))
            .Returns(questionDtoList).Verifiable();
        
        // Act
        var result = await _tagManagementService.GetQuestionsByTagIdAsync(tagId);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(questions.Count));
    }
    
    [Test]
    public async Task GetQuestionsByTagIdAsync_ShouldReturnEmptyList()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var questions = new List<Question>();
        var questionDtoList = new List<QuestionDto>();
        
        _unitOfWork.SetupGet(x => x.QuestionRepository)
            .Returns(_questionRepository.Object).Verifiable();
        
        _unitOfWork.Setup(x => x.QuestionRepository.GetQuestionsByTagIdAsync(tagId))
            .ReturnsAsync(questions).Verifiable();
        
        _mapper.Setup(x => x.Map<IList<QuestionDto>>(It.IsAny<IList<Question>>()))
            .Returns(questionDtoList).Verifiable();
        
        // Act
        var result = await _tagManagementService.GetQuestionsByTagIdAsync(tagId);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(questions.Count));
    }
    
    [Test]
    public async Task GetQuestionsByTagIdAsync_ShouldThrowException()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var exception = new Exception("An error occurred while fetching questions by tag id");
        
        _unitOfWork.SetupGet(x => x.QuestionRepository)
            .Returns(_questionRepository.Object).Verifiable();
        
        _unitOfWork.Setup(x => x.QuestionRepository.GetQuestionsByTagIdAsync(tagId))
            .ThrowsAsync(exception).Verifiable();
        
        // Act
        var ex = Assert.ThrowsAsync<Exception>(() => _tagManagementService.GetQuestionsByTagIdAsync(tagId));
        
        // Assert
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex.Message, Is.EqualTo(exception.Message));
    }
    
}