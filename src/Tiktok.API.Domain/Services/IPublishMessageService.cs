using Tiktok.API.Domain.EventBusMessages.Events;

namespace Tiktok.API.Domain.Services;

public interface IPublishMessageService

{
    Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : EventBase;
}