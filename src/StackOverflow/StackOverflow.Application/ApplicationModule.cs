using Autofac;
using StackOverflow.Application.Contracts.Features;
using StackOverflow.Application.Features.AccountManagementServices;
using StackOverflow.Application.Contracts.Features.AccountManagementServices;
using StackOverflow.Application.Contracts.Features.AwsManagementServices;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices;
using StackOverflow.Application.Contracts.Utilities;
using StackOverflow.Application.Features;
using StackOverflow.Application.Features.AwsManagementServices;
using StackOverflow.Application.Features.QuestionManagementServices;

namespace StackOverflow.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MemberManagementService>().As<IMemberManagementService>()
				.InstancePerLifetimeScope();
            
            builder.RegisterType<ImageManagementService>().As<IImageManagementService>()
				.InstancePerLifetimeScope();
            
            builder.RegisterType<QuestionManagementService>().As<IQuestionManagementService>()
				.InstancePerLifetimeScope();

            builder.RegisterType<DateTimeProvider>().As<IDateTimeProvider>()
                .InstancePerDependency();
            
            builder.RegisterType<GuidProvider>().As<IGuidProvider>()
                .InstancePerDependency();
            
            builder.RegisterType<TagManagementService>().As<ITagManagementService>()
                .InstancePerDependency();
            
            builder.RegisterType<NotificationService>().As<INotificationService>()
                .InstancePerDependency(); 
            
            builder.RegisterType<EmailQueueService>().As<IEmailQueueService>()
                .InstancePerDependency();
            
            builder.Register(c => new HttpClient()).SingleInstance();
            
            builder.RegisterType<RecaptchaService>().As<IRecaptchaService>()
                .InstancePerDependency();
        }
    }
}
