using Autofac.Extras.Moq;
using MapsterMapper;
using Moq;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;
using StackOverflow.Application.Contracts.Utilities;
using StackOverflow.Application.Features.QuestionManagementServices;
using StackOverflow.Domain.Repositories;
using StackOverflow.Web.Models.QuestionModels;

namespace StackOverflow.Application.Tests.ModelTests;

public class AskViewModelTests
{
    private AutoMock _mock;
    private Mock<IQuestionRepository> _questionRepository;
    private Mock<IMapper> _mapper;
    private Mock<IDateTimeProvider> _dateTimeProvider;
    private Mock<IApplicationUnitOfWork> _unitOfWork;
    private QuestionManagementService _questionManagementService;
    
    [SetUp]
    public void Setup()
    {
        _questionRepository = _mock.Mock<IQuestionRepository>();
        _mapper = _mock.Mock<IMapper>();
        _dateTimeProvider = _mock.Mock<IDateTimeProvider>();
        _unitOfWork = new Mock<IApplicationUnitOfWork>();
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
        _dateTimeProvider.Reset();
        _mapper.Reset();
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _mock.Dispose();
    }
    
    [Test]
    public async Task CreateQuestionAsync_WhenCalled_CreatesQuestion()
    {
        // Arrange
        var createdAt = DateTime.UtcNow;
        var askViewModel = new AskViewModel(_questionManagementService, _dateTimeProvider.Object, _mapper.Object);
        var createQuestionDto = new CreateQuestionDto(
            askViewModel.ApplicationUserId, askViewModel.Title, askViewModel.Body, askViewModel.Tags, createdAt
            );
        
        _mapper.Setup(m => m.Map<CreateQuestionDto>(askViewModel))
            .Returns(createQuestionDto).Verifiable();
        
        _dateTimeProvider.Setup(d => d.GetUtcNow())
            .Returns(DateTime.UtcNow).Verifiable();

        //await _questionManagementService.CreateQuestionAsync(createQuestionDto);
        
        // Act
        await askViewModel.CreateQuestionAsync();
        
        // Assert
        _dateTimeProvider.Verify(d => d.GetUtcNow(), Times.Once);
        _mapper.Verify(m => m.Map<CreateQuestionDto>(askViewModel), Times.Once);
        //_questionManagementService.(q => q.CreateQuestionAsync(createQuestionDto), Times.Once);
      }
}