using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using StackOverflow.Application.Contracts.Features.AwsManagementServices;
using StackOverflow.Application.Contracts.Properties;
using StackOverflow.Application.Contracts.Utilities;

namespace StackOverflow.Application.Features.AwsManagementServices;

public class EmailQueueService : IEmailQueueService
{
    private readonly IAmazonSQS _sqsClient;
    private readonly string _queueUrl;
    private readonly IEmailService _emailService;

    public EmailQueueService(IOptions<AwsSettings> awsSettings, IAmazonSQS sqsClient, IEmailService emailService)
    {
        _queueUrl = awsSettings.Value.QueueUrl;
        _sqsClient = sqsClient;
        _emailService = emailService;
    }
    
    public async Task EnqueueEmailAsync(EmailMessage emailMessage)
    {
        var messageBody = JsonSerializer.Serialize(emailMessage);
        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = _queueUrl,
            MessageBody = messageBody 
        };

        await _sqsClient.SendMessageAsync(sendMessageRequest);
    }

    public async Task ProcessMessagesAsync()
    {
        var receiveMessageRequest = new ReceiveMessageRequest
        {
            QueueUrl = _queueUrl,
            MaxNumberOfMessages = 10,
            WaitTimeSeconds = 20
        };

        var receiveMessageResponse = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest);

        foreach (var message in receiveMessageResponse.Messages)
        {
            var emailInfo = JsonSerializer.Deserialize<EmailMessage>(message.Body);

            if (emailInfo != null)
            {
                await _emailService.SendSingleEmail(emailInfo.RecipientEmail, emailInfo.RecipientEmail, emailInfo.Subject, emailInfo.Body);

                var deleteRequest = new DeleteMessageRequest
                {
                    QueueUrl = _queueUrl,
                    ReceiptHandle = message.ReceiptHandle
                };

                await _sqsClient.DeleteMessageAsync(deleteRequest);
            }
        }
    }
}