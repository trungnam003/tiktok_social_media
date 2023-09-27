namespace Tiktok.API.Domain.EventBusMessages.Interfaces;

public interface ISendMailForgotPasswordEvent : IEventBase
{
    public string Email { get; set; }
    public string Otp { get; set; }
    public string FullName { get; set; }
}