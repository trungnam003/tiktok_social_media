using Contracts.EventBusMessages.Events;
using Contracts.EventBusMessages.Interfaces;

namespace Tiktok.API.Domain.Services;

public interface IPublishMessageService

{
    Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : EventBase;
}