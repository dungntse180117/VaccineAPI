using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.Shared.Helpers;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace VaccineAPI.BusinessLogic.Services.Implement
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(string toEmail, string subject, string body)
        {
            // Retrieve email settings from configuration
            var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();

            if (emailSettings == null)
            {
                throw new InvalidOperationException("Email settings are not configured.");
            }

            // Create a MimeMessage
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailSettings.SenderName, emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("", toEmail)); // Empty name is okay
            message.Subject = subject;

            // Create a text part with HTML formatting
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            // Use SmtpClient to send the email
            using (var client = new SmtpClient())
            {
                // **Use SslOnConnect for port 465 (or if you want implicit SSL/TLS)**
                await client.ConnectAsync(emailSettings.SmtpServer, emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect); // Use SslOnConnect
                await client.AuthenticateAsync(emailSettings.SmtpUsername, emailSettings.SmtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}