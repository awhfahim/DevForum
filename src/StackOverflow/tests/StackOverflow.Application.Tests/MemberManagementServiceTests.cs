using Autofac.Extras.Moq;
using MapsterMapper;
using Moq;
using Shouldly;
using StackOverflow.Application.Contracts.Features.AccountManagementServices.MemberAggregateDTOs;
using StackOverflow.Application.Features.AccountManagementServices;
using StackOverflow.Domain.Entities;
using StackOverflow.Domain.Repositories;
namespace StackOverflow.Application.Tests
{
    public class MemberManagementServiceTests
    {
        private AutoMock _mock;
        private Mock<IApplicationUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private Mock<IMemberRepository> _mockMemberRepository;
        private MemberManagementService _memberManagementService;

        [SetUp]
        public void Setup() 
        {
            _mockUnitOfWork = _mock.Mock<IApplicationUnitOfWork>();
            _mockMapper = _mock.Mock<IMapper>();
            _mockMemberRepository = _mock.Mock<IMemberRepository>();
            _memberManagementService = _mock.Create<MemberManagementService>();
        }

        [TearDown] 
        public void TearDown() 
        {
            _mockUnitOfWork.Reset();
            _mockMapper.Reset();
            _mockMemberRepository.Reset();

        }
        [OneTimeSetUp] public void OneTimeSetUp() 
        {
            _mock = AutoMock.GetLoose();
        }
        [OneTimeTearDown] public void OneTimeTearDown() { _mock.Dispose(); }

        [Test]
        public async Task ConnectMemberTable_ShouldConnectMemberWithApplicationUser()
        {
            //Arrange
            var applicationUserId = Guid.NewGuid();
            const string? displayName = "fahim";

            _mockUnitOfWork.SetupGet(x => x.MemberRepository)
                .Returns(_mockMemberRepository.Object).Verifiable();

            _mockMemberRepository.Setup(x => x.AddAsync(It.IsAny<Member>()))
                .Returns(Task.CompletedTask).Verifiable();

            _mockUnitOfWork.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask).Verifiable();
                
                
            //Act

            await _memberManagementService.ConnectMemberTableAsync(applicationUserId, displayName);

            //Assert
            this.ShouldSatisfyAllConditions(
                () => _mockUnitOfWork.VerifyAll(),
                () => _mockMemberRepository.Verify(x => x.AddAsync(It.IsAny<Member>()), Times.Once)
            );
        }

        [Test]
        public async Task ConnectMemberTable_ShouldThrowArgumentNullException_WhenDisplayNameIsNull()
        {
            //Arrange
            var applicationUserId = Guid.NewGuid();
            string? displayName = null;

            _mockUnitOfWork.SetupGet(x => x.MemberRepository)
                .Returns(_mockMemberRepository.Object).Verifiable();

            _mockMemberRepository.Setup(x => x.AddAsync(It.IsAny<Member>()))
                .Returns(Task.CompletedTask).Verifiable();

            _mockUnitOfWork.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask).Verifiable();


            //Act & Assert

            await Should.ThrowAsync<NullReferenceException>(async 
                () => await _memberManagementService.ConnectMemberTableAsync(applicationUserId, displayName!));
        }

        [Test]
        public async Task UpdateMemberProfileAsync_ShouldUpdateMemberProfileSuccessfully()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var memberProfileDto = new MemberProfileDto(
                "Fahim",
                "Awh Fahim",
                "Mirpur 2",
                "text to time travel target toss",
                null,null,null
            );
            var member = new Member(userId, "Fahim")
            {
                Location = memberProfileDto.Location,
                AboutMe = memberProfileDto.AboutMe,
                FullName = memberProfileDto.FullName
            };

            _mockUnitOfWork.SetupGet(x => x.MemberRepository)
                .Returns(_mockMemberRepository.Object).Verifiable();

            _mockMemberRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(member).Verifiable();

            _mockMapper.Setup(x => x.Map(It.IsAny<MemberProfileDto>(), It.IsAny<Member>()))
                .Verifiable();

            _mockUnitOfWork.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask).Verifiable();

            //Act

            await _memberManagementService.UpdateMemberProfileAsync(userId, memberProfileDto);

            //Assert

            this.ShouldSatisfyAllConditions(
                () => _mockUnitOfWork.VerifyAll(),
                () => _mockMemberRepository.VerifyAll(),
                () => _mockMapper.VerifyAll()

            );
        }
    }
}
