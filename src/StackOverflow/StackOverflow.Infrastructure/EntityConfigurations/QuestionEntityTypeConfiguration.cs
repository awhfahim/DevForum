using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackOverflow.Domain.Entities;

namespace StackOverflow.Infrastructure.EntityConfigurations
{
	public class QuestionEntityTypeConfiguration : IEntityTypeConfiguration<Question>
	{
		public void Configure(EntityTypeBuilder<Question> builder)
		{
			builder.HasKey(q => q.Id);
			builder.Property(q => q.Title).IsRequired().HasMaxLength(250);
			builder.Property(q => q.Body).IsRequired().HasMaxLength(5000);
			builder.Property(q => q.MemberId).IsRequired();


			builder.HasMany<Answer>()
				.WithOne()
				.HasForeignKey(a => a.QuestionId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			builder.HasMany(x => x.QuestionComments)
				.WithOne()
				.HasForeignKey(qc => qc.QuestionId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			builder.HasMany(x => x.QuestionVotes)
				.WithOne()
				.HasForeignKey(qv => qv.QuestionId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			builder.HasMany(x => x.QuestionTags)
				.WithOne()
				.HasForeignKey(qt => qt.QuestionId)
				.OnDelete(DeleteBehavior.NoAction)
				.IsRequired();
		}
	}
}
