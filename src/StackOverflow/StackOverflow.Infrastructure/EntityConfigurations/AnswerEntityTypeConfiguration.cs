using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackOverflow.Domain.Entities;

namespace StackOverflow.Infrastructure.EntityConfigurations
{
	public class AnswerEntityTypeConfiguration : IEntityTypeConfiguration<Answer>
	{
		public void Configure(EntityTypeBuilder<Answer> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasMany(x => x.AnswerComments)
				.WithOne()
				.HasForeignKey(x => x.AnswerId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			builder.HasMany(x => x.AnswerVotes)
				.WithOne()
				.HasForeignKey(x=>x.AnswerId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		}
	}
}
