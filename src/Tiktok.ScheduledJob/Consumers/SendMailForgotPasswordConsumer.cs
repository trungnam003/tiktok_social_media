using MassTransit;
using Tiktok.API.Domain.EventBusMessages.Events;
using Tiktok.API.Domain.SeedWork;
using Tiktok.API.Domain.Services;
using Tiktok.ScheduledJob.Services.Interfaces;
using ILogger = Serilog.ILogger;

namespace Tiktok.ScheduledJob.Consumers;

public class SendMailForgotPasswordConsumer : IConsumer<SendMailForgotPasswordEvent>
{
    private readonly IEmailService<MailRequest> _emailService;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly IScheduleService _scheduleService;
    private readonly ILogger _logger;

    public SendMailForgotPasswordConsumer(IEmailService<MailRequest> emailService,
        IEmailTemplateService emailTemplateService, IScheduleService scheduleService, ILogger logger)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _emailTemplateService = emailTemplateService ?? throw new ArgumentNullException(nameof(emailTemplateService));
        _scheduleService = scheduleService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendMailForgotPasswordEvent> context)
    {
        _logger.Information($"Start {nameof(SendMailForgotPasswordConsumer)} - Email {context.Message.Email}");
        var message = context.Message;
        var template = await _emailTemplateService.GenerateOtpForgotPasswordEmail( message.Otp, message.FullName);
        var mailRequest = new MailRequest
        {
            To = message.Email,
            Subject = "Tiktok - Forgot Password",
            Body = template,
        };
        _scheduleService.Enqueue(() => _emailService.SendEmail(mailRequest));
        _logger.Information($"End {nameof(SendMailForgotPasswordConsumer)} - Email {context.Message.Email}");

    }
}