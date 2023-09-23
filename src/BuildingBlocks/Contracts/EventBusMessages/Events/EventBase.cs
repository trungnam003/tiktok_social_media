using Contracts.EventBusMessages.Interfaces;

namespace Contracts.EventBusMessages.Events;

public record EventBase() : IEventBase
{
    public DateTime CreatedAt { get; }= DateTime.UtcNow;
    public string Id { get; } = Guid.NewGuid().ToString();
}
