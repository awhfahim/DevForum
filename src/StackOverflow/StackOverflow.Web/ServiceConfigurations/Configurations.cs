using Mapster;
using Microsoft.AspNetCore.Components.Web;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.AnswerAggregateDTOs;
using StackOverflow.Application.Contracts.Features.QuestionManagementServices.QuestionAggregateDTOs;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.ActionFilters;
using StackOverflow.Web.Areas.Discussion.Models;

namespace StackOverflow.Web.ServiceConfigurations;

public static class Configurations
{
    public static void MapsterConfigurations(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        
        //Example Configuration
        config.NewConfig<AnswerViewModel, CreateAnswerDto>()
            .Map(dest => dest.Body, src => src.Body)
            .Map(dest => dest.QuestionId, src => src.QuestionId)
            .Map(dest => dest.AppliationUserId, src => src.ApplicationUserId);
        
        var constructorInfo = typeof(Answer).GetConstructor(new[]
        {
            typeof(Guid), 
            typeof(Guid),
            typeof(Guid),
            typeof(string),
            typeof(DateTime)
        });

        if (constructorInfo != null)
            config.NewConfig<CreateAnswerDto,Answer>()
                .MapToConstructor(constructorInfo);
        

        config.NewConfig<QuestionDto,Question>().Ignore(x => x.QuestionTags);

        config.NewConfig<(string Title, string Body, Guid Id), QuestionDto>()
            .Map(d => d.Body, s => s.Body)
            .Map(d => d.Title, s => s.Title)
            .Map(d => d.Id, s => s.Id);
    }
    
    public static void AddActionFilters(this IServiceCollection services)
    {
        services.AddTransient<GetApplicationUserIdActionFilter>();
    }
}