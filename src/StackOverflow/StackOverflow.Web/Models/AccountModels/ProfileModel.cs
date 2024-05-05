using Autofac;
using MapsterMapper;
using StackOverflow.Application.Contracts.Features.AccountManagementServices;
using StackOverflow.Application.Contracts.Features.AccountManagementServices.MemberAggregateDTOs;

namespace StackOverflow.Web.Models.AccountModels
{
    public class ProfileModel
    {
        private IMemberManagementService _memberManagementService;
        private IMapper _mapper;
        public string? DisplayName { get; init; }
        public string? FullName { get; init; }
        public string? Location { get; init; }
        public string? AboutMe { get; init; }
        public string? WebsiteLink { get; init; }
        public string? TwitterUsername { get; init; }
        public string? GitHubUsername { get; init; }

        public ProfileModel()
        {
        }
        public ProfileModel(IMemberManagementService memberManagementService, IMapper mapper)
        {
            _memberManagementService = memberManagementService;
            _mapper = mapper;
        }

        internal void Resolve(ILifetimeScope scope)
        {
            _memberManagementService = scope.Resolve<IMemberManagementService>();
            _mapper = scope.Resolve<IMapper>();
        }

        public async Task UpdateProfileAsync(Guid userId)
        {
            var memberProfileDto = _mapper.Map<MemberProfileDto>(this);
            await _memberManagementService.UpdateMemberProfileAsync(userId,memberProfileDto);
        }

        public async Task GetProfileAsync(string? getUserId)
        {
            var memberProfileDto = await _memberManagementService.GetMemberProfileAsync(Guid.Parse(getUserId!));
            _mapper.Map(memberProfileDto, this);
        }
        
    }
}
