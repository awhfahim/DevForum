using Microsoft.EntityFrameworkCore;
using StackOverflow.Domain.Entities;
using StackOverflow.Domain.Repositories;
using StackOverflow.Infrastructure.DbContexts;

namespace StackOverflow.Infrastructure.Repositories;

public class AnswerRepository : Repository<Answer, Guid>, IAnswerRepository
{
    public AnswerRepository(IApplicationDbContext context) : base((DbContext)context)
    {
    }

    public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid id)
      => await GetAsync(x => x, x => x.AnswerStatus == AnswerStatus.Accepted && x.QuestionId == id,null,
            x => x.Include(y => y.AnswerComments)
                .Include(y => y.AnswerVotes), true);
    
}