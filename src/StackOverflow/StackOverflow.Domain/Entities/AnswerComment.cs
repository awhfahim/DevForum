using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Domain.Entities
{
	public class AnswerComment : IEntity<Guid>
	{
		public Guid Id { get; set; }
		public Guid AnswerId { get; set; }
		public string Text { get; set; }
		public DateTime CreationTime { get; set; }
	}
}
