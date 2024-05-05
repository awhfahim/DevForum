using Autofac.Extras.Moq;
using MapsterMapper;
using Moq;
using Shouldly;
using StackOverflow.Application.Contracts.Features.NotificationDTOs;
using StackOverflow.Application.Features;
using StackOverflow.Domain.Entities;
using StackOverflow.Domain.Repositories;

namespace StackOverflow.Application.Tests;

public class NotificationServiceTests
{
    private AutoMock _mock;
    private Mock<IApplicationUnitOfWork> _unitOfWork;
    private Mock<INotificationRepository> _notificationRepository;
    private Mock<IMapper> _mapper;
    
    [SetUp]
    public void Setup()
    {
        _unitOfWork = _mock.Mock<IApplicationUnitOfWork>();
        _notificationRepository = _mock.Mock<INotificationRepository>();
        _mapper = _mock.Mock<IMapper>();
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
    public async Task GetUnreadNotifications_ShouldReturnUnreadNotifications()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notifications = new List<Notification> { new Notification { IsRead = false } };
        
        _unitOfWork.SetupGet(x => x.NotificationRepository)
            .Returns(_notificationRepository.Object).Verifiable();
        
        _unitOfWork.Setup(x => x.NotificationRepository.GetUnreadNotifications(userId))
            .ReturnsAsync(notifications).Verifiable();
        
        _mapper.Setup(x => x.Map<List<NotificationDto>>(notifications))
            .Returns(new List<NotificationDto>()).Verifiable();
        
        // Act
        var service = _mock.Create<NotificationService>();
        var result = await service.GetUnreadNotifications(userId);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        _mapper.Verify(x => x.Map<List<NotificationDto>>(notifications), Times.Once);
    }
    
    [Test]
    public async Task GetUnreadNotifications_ShouldReturnEmptyList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notifications = new List<Notification>();
        
        _unitOfWork.SetupGet(x => x.NotificationRepository)
            .Returns(_notificationRepository.Object).Verifiable();
        
        _unitOfWork.Setup(x => x.NotificationRepository.GetUnreadNotifications(userId))
            .ReturnsAsync(notifications).Verifiable();
        
        // Act
        var service = _mock.Create<NotificationService>();
        var result = await service.GetUnreadNotifications(userId);
        
        // Assert
        result.ShouldBeNull();
    }
}