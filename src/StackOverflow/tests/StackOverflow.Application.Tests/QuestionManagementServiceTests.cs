using Autofac.Extras.Moq;
using MapsterMapper;
using Moq;
using Shouldly;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.AnswerAggregateDTOs;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;
using StackOverflow.Application.Contracts.Utilities;
using StackOverflow.Application.Features.QuestionManagementServices;
using StackOverflow.Domain.Entities;
using StackOverflow.Domain.Exceptions;
using StackOverflow.Domain.Repositories;

namespace StackOverflow.Application.Tests;

public class QuestionManagementServiceTests
{
    private AutoMock _mock;
    private Mock<IQuestionRepository> _questionRepository;
    private Mock<IAnswerRepository> _answerRepository;
    private Mock<IApplicationUnitOfWork> _unitOfWork;
    private Mock<IGuidProvider> _guidProvider;
    private Mock<IMapper> _mapper;
    private Mock<ITagRepository> _tagRepository;
    private QuestionManagementService _questionManagementService;
    
    [SetUp]
    public void Setup()
    {
        _questionRepository = _mock.Mock<IQuestionRepository>();
        _answerRepository = _mock.Mock<IAnswerRepository>();
        _unitOfWork = _mock.Mock<IApplicationUnitOfWork>();
        _guidProvider = _mock.Mock<IGuidProvider>();
        _mapper = _mock.Mock<IMapper>();
        _tagRepository = _mock.Mock<ITagRepository>();
        _questionManagementService = _mock.Create<QuestionManagementService>();
    }
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _mock = AutoMock.GetLoose();
    }
    
    [TearDown]
    public void TearDown()
    {
        _questionRepository.Reset();
        _tagRepository.Reset();
        _answerRepository.Reset();
        _unitOfWork.Reset();
        _guidProvider.Reset();
        _mapper.Reset();
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _mock.Dispose();
    }

    [Test]
    public async Task CreateQuestionAsync_ShouldCreateQuestionSuccessfully()
    {
        // Arrange
        var createQuestionDto = new CreateQuestionDto(
            Guid.NewGuid(), 
            "Title", 
            "Body", 
            new List<string> { "Tag1", "Tag2" }, 
            DateTime.Now);
        
        var question = new Question(
            Guid.NewGuid(),
            Guid.NewGuid(),
            createQuestionDto.Title,
            createQuestionDto.Body,
            createQuestionDto.CreatedAt
        );
        
        _guidProvider.Setup(x => x.GetGuid()).Returns(question.Id).Verifiable();

        _unitOfWork.SetupGet(x => x.TagRepository)
            .Returns(_tagRepository.Object).Verifiable();
        
        _tagRepository.Setup(x => x.AddAsync(It.IsAny<Tag>()))
            .Returns(Task.CompletedTask).Verifiable();
        
        _tagRepository.Setup(x => x.GetTagByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((Tag)null!).Verifiable();
        
        _tagRepository.Setup(x => x.AddAsync(It.IsAny<Tag>()))
            .Returns(Task.CompletedTask).Verifiable();
        
        _unitOfWork.SetupGet(x => x.QuestionRepository)
            .Returns(_questionRepository.Object).Verifiable();
        
        _questionRepository.Setup(q => 
                q.AddAsync(It.IsAny<Question>())).Returns(Task.CompletedTask).Verifiable();
        
        _unitOfWork.Setup(u => 
                u.SaveAsync()).Returns(Task.CompletedTask).Verifiable();

        // Act
        await _questionManagementService.CreateQuestionAsync(createQuestionDto);
        
        // Assert
        this.ShouldSatisfyAllConditions(
            () => _guidProvider.VerifyAll(),
            () => _unitOfWork.VerifyAll(),
            () => _tagRepository.Verify(x => x.AddAsync(It.IsAny<Tag>()), Times.Exactly(2)),
            () => _tagRepository.Verify(x => x.GetTagByNameAsync(It.IsAny<string>()), Times.Exactly(2)),
            () => _questionRepository.Verify(q => q.AddAsync(It.IsAny<Question>()), Times.Once));
    }
    
    [Test]
    public async Task CreateQuestionAsync_ShouldLogError_WhenCreateQuestionDtoIsNull()
    {
        // Arrange
        CreateQuestionDto? createQuestionDto = null;

        // Act && Assert
        await Should.ThrowAsync<ArgumentNullException>(async
            () => await _questionManagementService.CreateQuestionAsync(createQuestionDto!)
        );
    }
    
    
    [Test]
    public async Task GetAllQuestionAsync_ReturnsAllQuestions_Successfully()
    {
        // Arrange
        const int pageNumber = 1;
        const int pageSize = 10;
        const string sortOption = "Newest";
        
        _unitOfWork.Setup(u => 
                u.GetQuestionsAsync(pageNumber, pageSize, sortOption))
            .ReturnsAsync((new List<QuestionRetrievalDto>(), 0)).Verifiable();
        // Act
        var (questions, total) = await _questionManagementService.GetAllQuestionAsync(pageNumber, pageSize, sortOption);
        // Assert
        questions.ShouldNotBeNull();
    }
    
    [Test]
    public async Task GetAllQuestionAsync_ReturnsEmptyList_WhenQuestionsIsNull()
    {
        // Arrange
        const int pageNumber = 1;
        const int pageSize = 10;
        const string sortOption = "Newest";
        
        _unitOfWork.Setup(u => 
                u.GetQuestionsAsync(pageNumber, pageSize, sortOption))
            .ReturnsAsync((null!, 0)).Verifiable();
        // Act
        var (questions, total) = await _questionManagementService.GetAllQuestionAsync(pageNumber, pageSize, sortOption);
        // Assert
        questions.ShouldBeNull();
    }
    
    [Test]
    public async Task GetQuestionByIdAsync_ShouldReturnQuestionById_Successfully()
    {
        // Arrange
         var id = Guid.NewGuid();
         var question = new Question(
                id,
                Guid.NewGuid(),
                "Title",
                "Body",
                DateTime.Now
         );
         
         question.AddQuestionTag(Guid.NewGuid());
         var tag = new Tag(Guid.NewGuid(),"Tag");
         
         _unitOfWork.SetupGet(x => x.QuestionRepository)
             .Returns(_questionRepository.Object).Verifiable();
         
         _questionRepository.Setup(x => x.GetQuestionWithCommentAsync(id))
             .ReturnsAsync(question).Verifiable();
         
         _unitOfWork.SetupGet(x => x.TagRepository)
             .Returns(_tagRepository.Object).Verifiable();
         
         _tagRepository.Setup(x => x.GetTagByIdAsync(It.IsAny<Guid>()))
             .ReturnsAsync(tag).Verifiable();
         
         _mapper.Setup(x => x.Map<QuestionDto>(It.IsAny<Question>()))
             .Returns(new QuestionDto()).Verifiable();
        // Act
        
        var result = await _questionManagementService.GetQuestionByIdAsync(id);
        
        // Assert
        result.ShouldNotBeNull();
    }
    
    [Test]
    public async Task GetQuestionByIdAsync_ShouldReturnNull_WhenQuestionIsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        _unitOfWork.SetupGet(x => x.QuestionRepository)
            .Returns(_questionRepository.Object).Verifiable();
        
        _questionRepository.Setup(x => x.GetQuestionWithCommentAsync(id))
            .ReturnsAsync((Question)null!).Verifiable();
        
        // Act
        var result = await _questionManagementService.GetQuestionByIdAsync(id);
        
        // Assert
        result.ShouldBeNull();
    }
    
    [Test]
    public async Task GetQuestionByIdAsync_ShouldReturnQuestion_WhenTagIsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var question = new Question(
            id,
            Guid.NewGuid(),
            "Title",
            "Body",
            DateTime.Now
        );
        
        _unitOfWork.SetupGet(x => x.QuestionRepository)
            .Returns(_questionRepository.Object).Verifiable();
        
        _questionRepository.Setup(x => x.GetQuestionWithCommentAsync(id))
            .ReturnsAsync(question).Verifiable();
        
        _mapper.Setup(x => x.Map<QuestionDto>(It.IsAny<Question>()))
            .Returns(new QuestionDto()).Verifiable();
        
        // Act
        var result = await _questionManagementService.GetQuestionByIdAsync(id);
        
        // Assert
        result.ShouldNotBeNull();
    }
    
    [Test]
    public async Task AddCommentAsync_ShouldAddCommentSuccessfully()
    {
        // Arrange
        var questionCommentDto = new QuestionCommentDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Comment",
            DateTime.Now
        );

        var question = new Question(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Title",
            "Body",
            DateTime.Now
        );
        
        _unitOfWork.SetupGet(x => x.QuestionRepository)
            .Returns(_questionRepository.Object).Verifiable();
        
        _questionRepository.Setup(x => x.GetByIdWithRelatedEntityAsync(It.IsAny<Guid>()))
            .ReturnsAsync(question).Verifiable();
        
        _unitOfWork.Setup(u => u.SaveAsync())
            .Returns(Task.CompletedTask).Verifiable();
        // Act
        await _questionManagementService.AddCommentAsync(questionCommentDto);
        // Assert
        this.ShouldSatisfyAllConditions(
            () => _unitOfWork.VerifyAll(),
            () => _questionRepository.VerifyAll());
    }
    
    [Test]
    public async Task AddCommentAsync_ShouldThrowNotFoundException_WhenQuestionIsNull()
    {
        // Arrange
        var questionCommentDto = new QuestionCommentDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Comment",
            DateTime.Now
        );
        _unitOfWork.SetupGet(x => x.QuestionRepository)
            .Returns(_questionRepository.Object).Verifiable();
        _questionRepository.Setup(x => x.GetByIdWithRelatedEntityAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Question)null!).Verifiable();
        
        // Act && Assert
        await Should.ThrowAsync<NotFoundException>(async
            () => await _questionManagementService.AddCommentAsync(questionCommentDto)
        );
    }

    [Test]
    public async Task CreateAnswerAsync_ShouldCreateAnswerSuccessfully()
    {
        // Arrange
        var createAnswerDto = new CreateAnswerDto()
        {
            Body = "new boyd",
            QuestionId = Guid.NewGuid(),
            AppliationUserId = Guid.NewGuid()
        };

        var answer = new Answer(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            createAnswerDto.Body,
            DateTime.Now
        );
        var memberId = Guid.NewGuid();

        _mapper.Setup(x => x.Map<Answer>(It.IsAny<CreateAnswerDto>()))
            .Returns(answer).Verifiable();

        _unitOfWork.SetupGet(x => x.AnswerRepository)
            .Returns(_answerRepository.Object).Verifiable();

        _answerRepository.Setup(x => x.AddAsync(It.IsAny<Answer>()))
            .Returns(Task.CompletedTask).Verifiable();

        _unitOfWork.Setup(x => x.SaveAsync())
            .Returns(Task.CompletedTask).Verifiable();
        
        //Act
        await _questionManagementService.CreateAnswerAsync(answerDto: createAnswerDto);
        
        //Arrange
        this.ShouldSatisfyAllConditions(
            () => _mapper.Verify(x => x.Map<Answer>(It.IsAny<CreateAnswerDto>()), Times.Once),
            () => _answerRepository.Verify(x => x.AddAsync(It.IsAny<Answer>()), Times.Once),
            () => _unitOfWork.Verify(x => x.SaveAsync(), Times.Once)
        );
    }
    
    [Test]
    public async Task CreateAnswerAsync_ShouldThrowArgumentNullException_WhenCreateAnswerDtoIsNull()
    {
        // Arrange
        CreateAnswerDto? createAnswerDto = null;
        
        // Act && Assert
        await Should.ThrowAsync<ArgumentNullException>(async
            () => await _questionManagementService.CreateAnswerAsync(createAnswerDto!)
        );
    }
    
    [Test]
    public async Task GetAnswerAsync_ShouldReturnAnswersSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        var answers = new List<Answer>
        {
            new Answer(
                Guid.NewGuid(),
                Guid.NewGuid(),
                id,
                "Body",
                DateTime.Now
            )
        };
        
        _unitOfWork.SetupGet(x => x.AnswerRepository)
            .Returns(_answerRepository.Object).Verifiable();
        
        _answerRepository.Setup(x => x.GetAnswersByQuestionIdAsync(id))
            .ReturnsAsync(answers).Verifiable();
        
        _mapper.Setup(x => x.Map<IEnumerable<CreateAnswerDto>>(It.IsAny<IEnumerable<Answer>>()))
            .Returns(new List<CreateAnswerDto>()).Verifiable();
        
        // Act
        var result = await _questionManagementService.GetAnswersAsync(id);
        
        // Assert
        result.ShouldNotBeNull();
    }

    [Test]
    public async Task CreateAnswerAsync_ShouldThrowDbUpdateException_WhenGetMemberIdAsyncReturnEmptyGuid()
    {
        
    }
    
    [Test]
    public async Task GetAnswerAsync_ShouldReturnEmptyList_WhenAnswersIsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        _unitOfWork.SetupGet(x => x.AnswerRepository)
            .Returns(_answerRepository.Object).Verifiable();
        
        _answerRepository.Setup(x => x.GetAnswersByQuestionIdAsync(id))
            .ReturnsAsync((List<Answer>)null!).Verifiable();
        
        // Act
        var result = await _questionManagementService.GetAnswersAsync(id);
        
        // Assert
        result.ShouldBeEmpty();
    }
    
    [Test]
    public async Task EditQuestionAsync_ShouldEditQuestionSuccessfully()
    {
        // Arrange
        var questionDto = new QuestionDto()
        {
            Id = Guid.NewGuid(),
            Title = "Title",
            Body = "Body",
            CreatedAt = DateTime.Now,
            QuestionTags = new List<QuestionTagDto>()
            {
                new QuestionTagDto("Tag"),
                new QuestionTagDto("Tag2")
            }
        };
        
        var question = new Question(
            questionDto.Id,
            Guid.NewGuid(),
            questionDto.Title,
            questionDto.Body,
            questionDto.CreatedAt
        );
        
        var tagNames = new List<string> { "Tag" };
        
        _unitOfWork.SetupGet(x => x.QuestionRepository)
            .Returns(_questionRepository.Object).Verifiable();
        
        _questionRepository.Setup(x => x.GetQuestionByIdWithTags(It.IsAny<Guid>()))
            .ReturnsAsync(question).Verifiable();
        
        _unitOfWork.SetupGet(x => x.TagRepository)
            .Returns(_tagRepository.Object).Verifiable();
        
        _tagRepository.Setup(x => x.GetTagByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((Tag)null!).Verifiable();
        
        _tagRepository.Setup(x => x.AddAsync(It.IsAny<Tag>()))
            .Returns(Task.CompletedTask).Verifiable();
        
        _unitOfWork.Setup(x => x.SaveAsync())
            .Returns(Task.CompletedTask).Verifiable();
        
        // Act
        await _questionManagementService.EditQuestionAsync(questionDto);
        
        // Assert
        this.ShouldSatisfyAllConditions(
            () => _unitOfWork.VerifyAll(),
            () => _questionRepository.VerifyAll(),
            () => _tagRepository.Verify(x => x.AddAsync(It.IsAny<Tag>()), Times.Exactly(2)),
            () => _tagRepository.Verify(x => x.GetTagByNameAsync(It.IsAny<string>()), Times.Exactly(2))
        );
    }
    
    [Test]
    public async Task EditQuestionAsync_ShouldThrowNotFoundException_WhenQuestionIsNull()
    {
        // Arrange
        var questionDto = new QuestionDto()
        {
            Id = Guid.NewGuid(),
            Title = "Title",
            Body = "Body",
            CreatedAt = DateTime.Now,
            QuestionTags = new List<QuestionTagDto>()
            {
                new QuestionTagDto("Tag"),
                new QuestionTagDto("Tag2")
            }
        };
        
        _unitOfWork.SetupGet(x => x.QuestionRepository)
            .Returns(_questionRepository.Object).Verifiable();
        
        _questionRepository.Setup(x => x.GetQuestionByIdWithTags(It.IsAny<Guid>()))
            .ReturnsAsync((Question)null!).Verifiable();
        
        // Act && Assert
        await Should.ThrowAsync<NotFoundException>(async
            () => await _questionManagementService.EditQuestionAsync(questionDto)
        );
    }
    
    [Test]
    public async Task EditQuestionAsync_ShouldThrowArgumentNullException_WhenQuestionDtoIsNull()
    {
        // Arrange
        QuestionDto? questionDto = null;
        
        // Act && Assert
        await Should.ThrowAsync<NullReferenceException>(async
            () => await _questionManagementService.EditQuestionAsync(questionDto!)
        );
    }
    
}