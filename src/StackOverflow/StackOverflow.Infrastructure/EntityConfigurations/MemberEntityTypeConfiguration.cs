using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackOverflow.Domain.Entities;

namespace StackOverflow.Infrastructure.EntityConfigurations
{
	public class MemberEntityTypeConfiguration : IEntityTypeConfiguration<Member>
	{
		public void Configure(EntityTypeBuilder<Member> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(e => e.Id).ValueGeneratedNever();
			builder.Property(e => e.DisplayName).HasMaxLength(50).IsRequired();
			builder.Property(e => e.FullName).HasMaxLength(100);
			builder.Property(e => e.Location).HasMaxLength(100);
			builder.Property(e => e.AboutMe).HasMaxLength(1000);
			builder.Property(e => e.WebsiteLink).HasMaxLength(100);
			builder.Property(e => e.TwitterUsername).HasMaxLength(15);
			builder.Property(e => e.GitHubUsername).HasMaxLength(39);
			

			builder.HasMany<Answer>()
				.WithOne()
				.HasForeignKey(e => e.MemberId)
				.OnDelete(deleteBehavior: DeleteBehavior.NoAction)
				.IsRequired();

			builder.HasMany<Question>()
				.WithOne()
				.HasForeignKey(e => e.MemberId)
				.OnDelete(deleteBehavior: DeleteBehavior.Cascade)
				.IsRequired();
		}
	}
}
