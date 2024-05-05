using Microsoft.EntityFrameworkCore;
using StackOverflow.Domain.Entities;
using StackOverflow.Domain.Repositories;
using StackOverflow.Infrastructure.DbContexts;

namespace StackOverflow.Infrastructure.Repositories;

public class TagRepository : Repository<Tag, Guid>, ITagRepository
{
    public TagRepository(IApplicationDbContext context) : base((DbContext)context)
    {
    }

    public async Task<Tag?> GetTagByNameAsync(string name)
       => await SingleOrDefaultAsync(x => x, x => x.Name == name);

    public async Task<Tag> GetTagByIdAsync(Guid questionTagId)
       => await SingleOrDefaultAsync<Tag>(x => x, x => x.Id == questionTagId);
}