using Microsoft.EntityFrameworkCore;
using StackOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Infrastructure.DbContexts
{
    public interface IApplicationDbContext
    {
        DbSet<Question> Questions { get; set; }
        DbSet<Answer> Answers { get; set; }
        DbSet<Member> Members { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<Notification> Notifications { get; set; }

        //DbSet<Badge> Badges { get; set; }
        //DbSet<Comment> Comments { get; set; }
        //DbSet<BadgeType> BadgeTypes { get; set; }
        //DbSet<QuestionVote> QuestionVotes { get; set; }
        //DbSet<AnswerVote> AnswerVotes { get; set; }
        //DbSet<QuestionTag> QuestionTag { get; set; }
        //DbSet<QuestionComment> QuestionComments { get; set; }
        //DbSet<AnswerComment> AnswerComments { get; set; }
    }
}
