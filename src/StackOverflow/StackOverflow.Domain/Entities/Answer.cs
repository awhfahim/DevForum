namespace StackOverflow.Domain.Entities;

public class Answer : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public Guid QuestionId { get; set; }
    public string Body { get; set; }
    public int Votes { get; set; }
    public DateTime AnsweredAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public AnswerStatus AnswerStatus { get; set; }
    public IList<AnswerComment> AnswerComments { get; set; }
    public IList<AnswerVote> AnswerVotes { get; set; }

    public Answer(Guid id, Guid memberId, Guid questionId, string body, DateTime answeredAt)
    {
		Id = id;
		MemberId = memberId;
		QuestionId = questionId;
		Body = body;
		Votes = 0;
		AnsweredAt = answeredAt;
		AnswerComments = new List<AnswerComment>();
        AnswerVotes = new List<AnswerVote>();
	}
    
    public void Edit(string body)
	{
		Body = body;
		EditedAt = DateTime.Now;
	}
	public void Upvote()
	{
		Votes++;
	}

	public void Downvote()
	{
		Votes--;
	}
}

public enum AnswerStatus
{
	Accepted,
	Unaccepted,
	Pending
}