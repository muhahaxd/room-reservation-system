using MediatR;
using Microsoft.Extensions.Logging;
using RRS.Domain.Events;

namespace RRS.App.Application.DomainEvents;
public class ReservationDeletedDomainEventHandler : INotificationHandler<ReservationDeletedDomainEvent>
{
    private readonly ILogger<ReservationDeletedDomainEventHandler> _logger;

    public ReservationDeletedDomainEventHandler(ILogger<ReservationDeletedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ReservationDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ReservationCreatedDomainEvent)} received. If reservation deletion would have any side effect the imlementation would goes here.");

        return Task.CompletedTask;
    }
}
