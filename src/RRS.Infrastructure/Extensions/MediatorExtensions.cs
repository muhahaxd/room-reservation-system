using MediatR;
using RSS.Domain.Entities;

namespace RRS.Infrastructure.Extensions;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEventAsync(this IMediator mediator, Entity entity, CancellationToken cancellationToken)
    {
        foreach (var domainEvent in entity.DomainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }
    }

    public static async Task DispatchDomainEventsAsync(this IMediator mediator, IEnumerable<Entity> entities, CancellationToken cancellationToken)
    {
        var domainEventsToSend = entities.SelectMany(it => it.DomainEvents);
        foreach (var domainEvent in domainEventsToSend)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }
    }
}