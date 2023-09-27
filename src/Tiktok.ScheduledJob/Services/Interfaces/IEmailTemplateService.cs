namespace Tiktok.ScheduledJob.Services.Interfaces;

public interface IEmailTemplateService
{
    Task<string> ReadEmailTemplateAsync(string templateName, string format = "html");
    Task<string> GenerateOtpForgotPasswordEmail(string otp, string username);
}