using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using StackOverflow.Application.Contracts.Utilities;

namespace StackOverflow.Infrastructure.Email
{
    public class HtmlEmailService : IEmailService
    {
        private readonly Smtp _emailSettings;

        public HtmlEmailService(IOptions<Smtp> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<string> SendSingleEmail(string receiverName, string receiverEmail,
            string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_emailSettings.SenderName,
                _emailSettings.SenderEmail));

            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            client.Timeout = 30000;
            await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);

            var r = await client.SendAsync(message);
            await client.DisconnectAsync(true);
            return r;
        }
    }
}
