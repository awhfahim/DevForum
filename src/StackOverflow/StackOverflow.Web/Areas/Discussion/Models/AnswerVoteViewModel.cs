using Autofac;

namespace StackOverflow.Web.Areas.Discussion.Models;

public class AnswerVoteViewModel
{
    public Guid AnswerId { get; set; }
    public Guid QuestionId { get; set; }
    public void Resolve(ILifetimeScope scope)
    {
        throw new NotImplementedException();
    }

    public async Task AddVoteAsync()
    {
        throw new NotImplementedException();
    }
}