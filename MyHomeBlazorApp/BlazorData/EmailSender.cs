using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MyHomeBlazorApp.BlazorData;
using MimeKit;
using MailKit.Net.Smtp;

namespace WebPWrecover.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       ILogger<EmailSender> logger)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
    }

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(Options.MyHomeGmailPassword))
        {
            throw new Exception("Null MailSenderKey");
        }
        await Execute(Options.MyHomeGmailPassword, subject, message, toEmail);
    }
    // Todo: Add response to _logger from smtp client. Example:
    //    var response = await client.SendEmailAsync(msg);
    //    _logger.LogInformation(response.IsSuccessStatusCode
    //                           ? $"Email to {toEmail} queued successfully!"
    //                           : $"Failure Email to {toEmail}");

    public async Task<string> Execute(string apiKey, string subject, string message, string toEmail)
    {
        using var smtp = new SmtpClient();
        var msg = new MimeMessage();
        try
        {
            //Message
            msg.From.Add(MailboxAddress.Parse("yourhomesupervisor@gmail.com"));
            msg.To.Add(MailboxAddress.Parse(toEmail));
            msg.Subject = subject;
            var builder = new BodyBuilder();
            // builder.HtmlBody = EmailRequest.Body;
            builder.TextBody = message;
            msg.Body = builder.ToMessageBody();

            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            //if (!string.IsNullOrEmpty(username))
                //                    smtp.Authenticate(username, password);
                await smtp.AuthenticateAsync("yourhomesupervisor@gmail.com", apiKey);
            await smtp.SendAsync(msg);
            msg.Dispose();
            await smtp.DisconnectAsync(true);

            return "Email sent successfully!";
        }
        catch (Exception ex)
        {
            {
                return $"Email sending failed. Error: {ex.Message}";
            }
        }
       
    }
}
