namespace Contracts.Services;

public interface IEmailService<in T> where T : class
{
    Task SendEmailAsync(T request, CancellationToken cancellationToken = default);
    void SendEmail(T request);
}