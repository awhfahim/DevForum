using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Domain.Entities
{
	public class QuestionComment : IEntity<Guid>
	{
		public Guid Id { get; set; }
		public Guid MemberId { get; set; }
		public Guid QuestionId { get; set; }
		public string Text { get; set; }
		public DateTime CreationTime { get; set; }

		public QuestionComment()
		{
			
		}
        public QuestionComment(Guid memberId,Guid questionId, string text, DateTime creationTime)
		{
			MemberId = memberId;
			QuestionId = questionId;
			Text = text;
			CreationTime = creationTime;
		}
	}
}
