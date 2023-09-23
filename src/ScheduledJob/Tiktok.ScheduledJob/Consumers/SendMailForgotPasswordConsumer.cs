using Contracts.EventBusMessages.Events;
using Contracts.Services;
using MassTransit;
using Shared.SeedWork;
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
        // delay 20s
        await Task.Delay(10000);
        _logger.Information("End 20s sleep");
        var message = context.Message;
        var template = await _emailTemplateService.GenerateOtpForgotPasswordEmail( message.Otp, message.FullName);
        var mailRequest = new MailRequest
        {
            To = message.Email,
            Subject = "Tiktok - Forgot Password",
            Body = template,
            
        };
        _scheduleService.Schedule(() => _emailService.SendEmail(mailRequest)
        , TimeSpan.FromSeconds(20));
    }
}