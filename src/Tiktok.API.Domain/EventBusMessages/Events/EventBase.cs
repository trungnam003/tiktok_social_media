using Tiktok.API.Domain.EventBusMessages.Interfaces;

namespace Tiktok.API.Domain.EventBusMessages.Events;

public record EventBase() : IEventBase
{
    public DateTime CreatedAt { get; }= DateTime.UtcNow;
    public string Id { get; } = Guid.NewGuid().ToString();
}
