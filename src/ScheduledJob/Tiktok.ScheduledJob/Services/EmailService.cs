using Contracts.Services;
using Infrastructure.Configurations;
using MailKit.Net.Smtp;
using MimeKit;
using Shared.SeedWork;
using ILogger = Serilog.ILogger;

namespace Tiktok.ScheduledJob.Services;

public class EmailService : IEmailService<MailRequest>
{
    private readonly ILogger _logger;
    private readonly EmailSettings _emailSettings;
    public EmailService(ILogger logger, EmailSettings emailSettings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _emailSettings = emailSettings ?? throw new ArgumentNullException(nameof(emailSettings));
    }

    public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = default)
    {
        var mimeMessage = GetMimeMessage(request);
        using var smtpClient = new SmtpClient();
        try
        {
            _logger.Information($"Send email to {request.To}");
            await smtpClient.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, _emailSettings.UseSsl, cancellationToken);
            await smtpClient.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password, cancellationToken);
            await smtpClient.SendAsync(mimeMessage, cancellationToken);
            _logger.Information($"Send email success to {request.To}");

        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error while sending email");
        }
        finally
        {
            await smtpClient.DisconnectAsync(true, cancellationToken);
            smtpClient.Dispose();
        }
    }

    public void SendEmail(MailRequest request)
    {
        var emailMessage = GetMimeMessage(request);
        using var smtpClient = new SmtpClient();

        try
        {
            smtpClient.Connect(_emailSettings.SmtpServer, _emailSettings.Port, _emailSettings.UseSsl);
            smtpClient.Authenticate(_emailSettings.Username, _emailSettings.Password);
            smtpClient.Send(emailMessage);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message, ex);
        }
        finally
        {
            smtpClient.Disconnect(true);
            smtpClient.Dispose();
        }
    }

    private MimeMessage GetMimeMessage(MailRequest request)
    {
        var mimeMessage = new MimeMessage()
        {
            Sender = new MailboxAddress(_emailSettings.DisplayName, request.From ?? _emailSettings.From),
            Subject = request.Subject,
            Body = new BodyBuilder
            {
                HtmlBody = request.Body
            }.ToMessageBody()
        };
        
        if (request.ToAddAddress != null && request.ToAddAddress.Any())
        {
            foreach (var toAddress in request.ToAddAddress)
            {
                mimeMessage.To.Add(MailboxAddress.Parse(toAddress));
            }
        }
        else
        {
            var toAddress = request.To;
            mimeMessage.To.Add(MailboxAddress.Parse(toAddress));
        }

        return mimeMessage;
    }
}