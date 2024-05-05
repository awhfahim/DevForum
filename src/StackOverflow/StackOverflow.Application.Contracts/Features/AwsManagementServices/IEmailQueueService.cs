using StackOverflow.Application.Contracts.Properties;

namespace StackOverflow.Application.Contracts.Features.AwsManagementServices;

public interface IEmailQueueService
{
    Task EnqueueEmailAsync(EmailMessage emailMessage);
    Task ProcessMessagesAsync();
}