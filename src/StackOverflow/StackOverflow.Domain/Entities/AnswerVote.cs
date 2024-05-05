using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Domain.Entities
{
	public class AnswerVote : IEntity<Guid>
	{
		public Guid Id { get; set; }
		public Guid AnswerId { get; set; }
		public Guid MemberId { get; set; }
		public VoteType VoteType { get; set; }
	}
}
