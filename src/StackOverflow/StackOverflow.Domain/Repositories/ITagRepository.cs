using StackOverflow.Domain.Entities;

namespace StackOverflow.Domain.Repositories;

public interface ITagRepository : IRepositoryBase<Tag, Guid>
{
    Task<Tag?> GetTagByNameAsync(string tag);
    Task<Tag> GetTagByIdAsync(Guid questionTagTagId);
}