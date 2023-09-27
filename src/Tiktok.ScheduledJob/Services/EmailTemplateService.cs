using Tiktok.ScheduledJob.Services.Interfaces;

namespace Tiktok.ScheduledJob.Services;

public class EmailTemplateService : IEmailTemplateService
{
    private static readonly string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
    private static readonly string TemplateDirFolder = Path.Combine(BaseDir, "EmailTemplates");

    public async Task<string> ReadEmailTemplateAsync(string templateName, string format = "html")
    {
        var filePath = Path.Combine(TemplateDirFolder, $"{templateName}.{format}");
        await using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var sr = new StreamReader(fs);
        var result = await sr.ReadToEndAsync();
        return result;
    }

    public async Task<string> GenerateOtpForgotPasswordEmail(string otp, string username)
    {
        var template = await ReadEmailTemplateAsync("otp-forgot-password");
        template = template.Replace("{{code}}", otp);
        template = template.Replace("{{username}}", username);
        return template;
    }
}