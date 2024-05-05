namespace StackOverflow.Application.Contracts.Properties;

public class EmailMessage
{
    public string RecipientEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    
    public EmailMessage(string recipientEmail, string subject, string body)
    {
        RecipientEmail = recipientEmail;
        Subject = subject;
        Body = body;
    }
}
