namespace StackOverflow.Domain.Entities;

public class Question : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public uint AnswersCount { get; set; }
    public int VotesCount { get; set; }
    public IList<QuestionComment> QuestionComments { get; set; }
    public IList<QuestionTag> QuestionTags { get; set; }
    public IList<QuestionVote> QuestionVotes { get; set; }
    public Question(Guid id, Guid memberId, string title, string body,DateTime createdAt)
	{
		Id = id;
		MemberId = memberId;
		Title = title;
		Body = body;
		CreatedAt = createdAt;
		QuestionComments = new List<QuestionComment>();
		QuestionTags = new List<QuestionTag>();
        QuestionVotes = new List<QuestionVote>();
	}

	public void AddQuestionComment(QuestionComment questionComment)
	{
		QuestionComments.Add(questionComment);
	}
    
    public void AddQuestionTag(Guid tagId)
	{
		QuestionTags.Add(new QuestionTag(Id, tagId));
	}

	public void AddQuestionVote(Guid memberId, VoteType voteType)
	{
		QuestionVotes.Add(new QuestionVote(memberId, voteType, Id));
	}
	
	public void RemoveQuestionVote(Guid memberId)
	{
		var vote = QuestionVotes.FirstOrDefault(x => x.MemberId == memberId);
		if (vote != null)
		{
			QuestionVotes.Remove(vote);
		}
	}
}