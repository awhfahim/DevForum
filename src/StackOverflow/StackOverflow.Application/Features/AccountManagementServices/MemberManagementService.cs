using MapsterMapper;
using StackOverflow.Domain.Entities;
using StackOverflow.Application.Contracts.Features.AccountManagementServices;
using StackOverflow.Application.Contracts.Features.AccountManagementServices.MemberAggregateDTOs;
using StackOverflow.Domain.Exceptions;

namespace StackOverflow.Application.Features.AccountManagementServices
{
	public class MemberManagementService : IMemberManagementService
	{
		private readonly IApplicationUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public MemberManagementService(IApplicationUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task ConnectMemberTableAsync(Guid applicationUserId, string displayName)
		{
			if(displayName is null)
				throw new NullReferenceException(nameof(displayName));

            await _unitOfWork.MemberRepository.AddAsync(new Member(applicationUserId, displayName));
			await _unitOfWork.SaveAsync();
        }

		public async Task UpdateMemberProfileAsync(Guid userId, MemberProfileDto memberProfileDto)
		{
			var memberId = userId;
			var member = await _unitOfWork.MemberRepository.GetByIdAsync(memberId);

			if (member == null)
				throw new NotFoundException(nameof(member),memberId.ToString());

			_mapper.Map(memberProfileDto, member);
			await _unitOfWork.SaveAsync();
		}

		public async Task<MemberProfileDto> GetMemberProfileAsync(Guid userId)
		{
			var memberId = userId;
			var member = await _unitOfWork.MemberRepository.GetByIdAsync(memberId);
			var memberProfileDto = _mapper.Map<MemberProfileDto>(member);
			return memberProfileDto;
		}

		public async Task<int> GetReputationAsync(Guid userId)
		{
			var member = await _unitOfWork.MemberRepository.GetByIdAsync(userId);
			return member.Reputation;
		}
	}
}
