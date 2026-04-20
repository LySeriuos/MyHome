using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MyHomeBlazorApp.BlazorData;

namespace WebPWrecover.Services;

public class EmailSender : IEmailSender, IEmailSender<MyHomeBlazorAppUser>
{
    private readonly ILogger _logger;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       ILogger<EmailSender> logger)
    {
        Options = optionsAccessor.Value;
        _logger = logger;

        if (string.IsNullOrEmpty(Options.MyHomeGmailPassword))
        {
            _logger.LogWarning("BOOTSTRAP: MyHomeGmailPassword is EMPTY or NULL.");
        }
        else
        {
            // Safety log: shows it exists without showing the value
            _logger.LogInformation("BOOTSTRAP: MyHomeGmailPassword found (Length: {Length})",
                Options.MyHomeGmailPassword.Length);
        }
    }

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

   

    // --- Identity Specific Methods ---
    public async Task SendConfirmationLinkAsync(MyHomeBlazorAppUser user, string email, string confirmationLink) =>
        await SendEmailAsync(email, "Confirm your email", $"Please confirm your account by clicking here: {confirmationLink}");

    public async Task SendPasswordResetLinkAsync(MyHomeBlazorAppUser user, string email, string resetLink) =>
        await SendEmailAsync(email, "Reset your password", $"Please reset your password by clicking here: {resetLink}");

    public async Task SendPasswordResetCodeAsync(MyHomeBlazorAppUser user, string email, string resetCode) =>
        await SendEmailAsync(email, "Reset your password", $"Your reset code is: {resetCode}");
    // ---------------------------------

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(Options.MyHomeGmailPassword))
        {
            _logger.LogError("Null MailSenderKey - Gmail password not found in configuration.");
            return;
        }
        _logger.LogInformation("ATTEMPTING SEND: Using Gmail SMTP for {Email}...", toEmail);
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

            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            //if (!string.IsNullOrEmpty(username))
            //                    smtp.Authenticate(username, password);
            await smtp.AuthenticateAsync("yourhomesupervisor@gmail.com", apiKey);
            await smtp.SendAsync(msg);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation($"Email to {toEmail} sent successfully!");
            return "Email sent successfully!";
        }
        catch (Exception ex)
        {
            {
                _logger.LogError($"Email sending failed to {toEmail}. Error: {ex.Message}");
                return $"Email sending failed. Error: {ex.Message}";
            }
        }

    }
}

