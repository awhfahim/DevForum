using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackOverflow.Domain.Entities;
using StackOverflow.Infrastructure.Membership;

namespace StackOverflow.Infrastructure.EntityConfigurations
{
    public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.ProfileImageKey).HasMaxLength(100);
            builder.HasOne<Member>()
                .WithOne()
                .HasForeignKey<Member>(x => x.ApplicationUserId)
                .IsRequired();
        }
    }
}
