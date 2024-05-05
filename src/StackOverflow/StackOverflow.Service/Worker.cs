using StackOverflow.Application.Contracts.Features.AwsManagementServices;

namespace StackOverflow.Service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IEmailQueueService _emailQueueService;

    public Worker(ILogger<Worker> logger, IEmailQueueService emailQueueService)
    {
        _logger = logger;
        _emailQueueService = emailQueueService;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
           await _emailQueueService.ProcessMessagesAsync();
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(21000, stoppingToken);
        }
    }
}