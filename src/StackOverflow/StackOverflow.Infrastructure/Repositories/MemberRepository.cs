using Microsoft.EntityFrameworkCore;
using StackOverflow.Domain.Entities;
using StackOverflow.Domain.Repositories;
using StackOverflow.Infrastructure.DbContexts;

namespace StackOverflow.Infrastructure.Repositories
{
	public class MemberRepository : Repository<Member, Guid>, IMemberRepository
	{
		public MemberRepository(IApplicationDbContext context) : base((DbContext)context)
		{
		}
		public async Task<Guid> GetMemberIdByApplicationUserIdAsync(Guid applicationUserid)
			=> await SingleOrDefaultAsync(x => x.Id, x => x.ApplicationUserId == applicationUserid);
	}
}
  