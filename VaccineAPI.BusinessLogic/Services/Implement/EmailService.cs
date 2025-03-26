using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    // 1️⃣ Send Feedback Request Email (After Visit)
    public async Task SendFeedbackRequestEmailAsync(string recipientEmail, string userName, int visitId)
    {
        string feedbackUrl = $"http://yourwebsite.com/feedback?visitId={visitId}";

        var subject = "We value your feedback!";
        var body = $@"
            <h2>Hello {userName},</h2>
            <p>We hope your visit went well! Please share your feedback <a href='{feedbackUrl}'>here</a>.</p>
            <p>Thank you!</p>";

        await SendEmailAsync(recipientEmail, subject, body);
    }

    // 2️⃣ Send Thank-You Email (After Feedback)
    public async Task SendThankYouEmail(string recipientEmail, string userName)
    {
        var subject = "Thank You for Your Feedback!";
        var body = $@"
            <h2>Hello {userName},</h2>
            <p>We appreciate your time in sharing feedback. Your input helps us improve our services!</p>
            <p>Thank you!</p>";

        await SendEmailAsync(recipientEmail, subject, body);
    }

    // 🔥 Generic Email Sending Method
    private async Task SendEmailAsync(string recipientEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient(_config["EmailSettings:SmtpServer"])
        {
            Port = int.Parse(_config["EmailSettings:Port"]!),
            Credentials = new NetworkCredential(
                _config["EmailSettings:SenderEmail"],
                _config["EmailSettings:SenderPassword"]
            ),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_config["EmailSettings:SenderEmail"]!),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(recipientEmail);
        await smtpClient.SendMailAsync(mailMessage);
    }
}
