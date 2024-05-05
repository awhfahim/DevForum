using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Domain.Entities
{
	public class QuestionTag
	{
		public Guid QuestionId { get; set; }
		public Guid TagId { get; set; }

		public QuestionTag(Guid questionId, Guid tagId)
		{
			QuestionId = questionId;
			TagId = tagId;
		}
	}

}
