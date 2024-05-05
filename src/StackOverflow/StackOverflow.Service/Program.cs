using Amazon.SQS;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using StackOverflow.Application;
using StackOverflow.Application.Contracts.Properties;
using StackOverflow.Infrastructure;
using StackOverflow.Infrastructure.Email;
using StackOverflow.Service;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false)
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Application Starting up");

    var host = Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .UseSerilog()
        .ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.RegisterModule(new ApplicationModule());
            builder.RegisterModule(new InfrastructureModule());
        })
        .ConfigureServices(services =>
        {
            services.AddHostedService<Worker>();
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonSQS>();
            services.Configure<AwsSettings>(configuration.GetSection("AWS"));
            services.Configure<Smtp>(configuration.GetSection("Smtp"));
        })
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}