namespace Tiktok.API.Domain.EventBusMessages.Interfaces;

public interface IEventBase
{
    public DateTime CreatedAt { get;  }
    public string Id { get;}
}