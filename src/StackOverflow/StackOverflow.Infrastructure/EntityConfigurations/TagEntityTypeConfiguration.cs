using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackOverflow.Domain.Entities;

namespace StackOverflow.Infrastructure.EntityConfigurations
{
	public class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
	{
		public void Configure(EntityTypeBuilder<Tag> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).IsRequired().HasMaxLength(30);

			builder.HasMany<QuestionTag>()
				.WithOne()
				.HasForeignKey(x => x.TagId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		}
	}
}
