using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Domain.Entities
{
	public class QuestionVote : IEntity<Guid>
	{
		public Guid Id { get; set; }
		public Guid QuestionId { get; set; }
		public Guid MemberId { get; set; }
		public VoteType VoteType { get; set; }

		public QuestionVote(Guid memberId, VoteType voteType, Guid questionId)
		{
			MemberId = memberId;
			VoteType = voteType;
			QuestionId = questionId;
		}
	}

	public enum VoteType 
	{
		UpVote = 1, DownVote = 2
	}
}
