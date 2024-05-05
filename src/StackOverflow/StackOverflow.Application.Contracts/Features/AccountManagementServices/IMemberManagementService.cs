using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackOverflow.Application.Contracts.Features.AccountManagementServices.MemberAggregateDTOs;

namespace StackOverflow.Application.Contracts.Features.AccountManagementServices
{
	public interface IMemberManagementService
	{
        Task ConnectMemberTableAsync(Guid applicationUserId, string displayName);
        Task UpdateMemberProfileAsync(Guid userId, MemberProfileDto memberProfileDto);
        Task<MemberProfileDto> GetMemberProfileAsync(Guid memberId);
        Task<int> GetReputationAsync(Guid userId);
	}
}
