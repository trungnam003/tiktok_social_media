using Contracts.EventBusMessages.Events;
using Contracts.EventBusMessages.Interfaces;
using MassTransit;
using Tiktok.API.Domain.Services;

namespace Tiktok.API.Infrastructure.Services;

public class PublishMessageService : IPublishMessageService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishMessageService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }


    public Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : EventBase
    {
        return _publishEndpoint.Publish<T>(message, cancellationToken);
    }
}