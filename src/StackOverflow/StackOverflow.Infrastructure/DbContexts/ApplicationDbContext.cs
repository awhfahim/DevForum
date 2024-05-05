using FirstDemo.Infrastructure.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackOverflow.Domain.Entities;
using StackOverflow.Infrastructure.EntityConfigurations;
using StackOverflow.Infrastructure.Membership;

namespace StackOverflow.Infrastructure.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

		public DbSet<Question> Questions { get; set; }
		public DbSet<Answer> Answers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
			base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationUserEntityTypeConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(MemberEntityTypeConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(QuestionEntityTypeConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(AnswerEntityTypeConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(TagEntityTypeConfiguration).Assembly);

            builder.Entity<QuestionTag>(b =>
            {
                b.ToTable("QestionTags");
                
                b.HasKey(x => new { x.QuestionId, x.TagId });
            });
        }
    }
}
