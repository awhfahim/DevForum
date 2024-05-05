using Autofac;
using StackOverflow.Web.Areas.Discussion.Models;
using StackOverflow.Web.Models.AuthModels;
using StackOverflow.Web.Models.NotificationModels;
using StackOverflow.Web.Models.QuestionModels;

namespace StackOverflow.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AskViewModel>().AsSelf()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<AnswerViewModel>().AsSelf()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<RegisterModel>().AsSelf()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<NotificationListModel>().AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
