using Autofac;
using StackOverflow.Application;
using StackOverflow.Application.Contracts.Utilities;
using StackOverflow.Domain.Repositories;
using StackOverflow.Infrastructure.DbContexts;
using StackOverflow.Infrastructure.Email;
using StackOverflow.Infrastructure.Repositories;
using StackOverflow.Infrastructure.UnitOfWorks;

namespace StackOverflow.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => c.Resolve<ApplicationDbContext>())
                .As<IApplicationDbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<HtmlEmailService>().As<IEmailService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<MemberRepository>().As<IMemberRepository>()
                .InstancePerLifetimeScope(); 
            
            builder.RegisterType<QuestionRepository>().As<IQuestionRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<TagRepository>().As<ITagRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<AnswerRepository>().As<IAnswerRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<NotificationRepository>().As<INotificationRepository>()
                .InstancePerLifetimeScope();


            //         builder.RegisterType<TokenService>().As<ITokenService>()
            //            .InstancePerLifetimeScope();
        }
    }
}
