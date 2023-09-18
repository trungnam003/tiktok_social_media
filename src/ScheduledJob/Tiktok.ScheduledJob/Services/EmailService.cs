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
    private readonly SmtpClient _smtpClient;

    public EmailService(ILogger logger, EmailSettings emailSettings, SmtpClient smtpClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _emailSettings = emailSettings ?? throw new ArgumentNullException(nameof(emailSettings));
        _smtpClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));
    }

    public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = default)
    {
        var mimeMessage = GetMimeMessage(request);
        try
        {
            _logger.Information($"Send email to {request.To}");
            await _smtpClient.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, _emailSettings.UseSsl, cancellationToken);
            await _smtpClient.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password, cancellationToken);
            await _smtpClient.SendAsync(mimeMessage, cancellationToken);
            _logger.Information($"Send email success to {request.To}");

        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error while sending email");
        }
        finally
        {
            await _smtpClient.DisconnectAsync(true, cancellationToken);
            _smtpClient.Dispose();
        }
    }

    public void SendEmail(MailRequest request)
    {
        var emailMessage = GetMimeMessage(request);

        try
        {
            _smtpClient.Connect(_emailSettings.SmtpServer, _emailSettings.Port, _emailSettings.UseSsl);
            _smtpClient.Authenticate(_emailSettings.Username, _emailSettings.Password);
            _smtpClient.Send(emailMessage);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message, ex);
        }
        finally
        {
            _smtpClient.Disconnect(true);
            _smtpClient.Dispose();
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