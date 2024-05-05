using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using StackOverflow.Domain.Entities;
using StackOverflow.Domain.Repositories;
using StackOverflow.Infrastructure.DbContexts;

namespace StackOverflow.Infrastructure.Repositories;

public class QuestionRepository : Repository<Question, Guid>, IQuestionRepository
{
     //private readonly DbContext _dbContext;
    // protected readonly DbSet<Question> EntityDbSet;
    // protected readonly DatabaseFacade DbFacade;
    public QuestionRepository(IApplicationDbContext context) : base((DbContext)context)
    {
         //_dbContext = context as DbContext ?? throw new ArgumentNullException(nameof(context));
        // EntityDbSet = _dbContext.Set<Question>();
        // DbFacade = context.Database;
    }

    public async Task<Question> GetQuestionWithCommentAsync(Guid id)
            => await SingleOrDefaultAsync(x => x, x => x.Id == id, null,
            x => x.Include(y => y.QuestionComments)
                .Include(y => y.QuestionTags),
            true);

    public async Task<Question> GetByIdWithRelatedEntityAsync(Guid questionId)
        => await SingleOrDefaultAsync<Question>(x => x, y => y.Id == questionId,
            null, x => x.Include(y => y.QuestionComments.Take(1)), 
            false);

    public async Task<Question> GetQuestionByIdWithTags(Guid questionId)
        => await SingleOrDefaultAsync<Question>(x => x, x => x.Id == questionId,
            null, x => x.Include(y => y.QuestionTags),
            true);

    public async Task<Question> GetQuestionByIdWithVotes(Guid questionId, Guid memberId)
        => await SingleOrDefaultAsync<Question>(x => x, x => x.Id == questionId,
            null, x => x.Include(y => 
                y.QuestionVotes.Where(z => z.MemberId == memberId)),
            false);

    public async Task<IEnumerable<(string Title, string Body, Guid Id)>> GetQuestionsAsync(Guid userId)
        => (await GetAsync(x => new { x.Title, x.Body, x.Id }, x => x.MemberId == userId))
            .Select(x => (x.Title,x.Body, x.Id)).ToList();

    public async Task<IList<Question>> GetQuestionsByTagIdAsync(Guid tagId)
       => (await GetAsync(x => x, x =>x.QuestionTags.Any(y => y.TagId == tagId))).ToList();
}