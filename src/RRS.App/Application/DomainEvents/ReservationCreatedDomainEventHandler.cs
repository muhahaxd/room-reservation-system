using MediatR;
using Microsoft.Extensions.Logging;
using RRS.Domain.Events;

namespace RRS.App.Application.DomainEvents;
public class ReservationCreatedDomainEventHandler : INotificationHandler<ReservationCreatedDomainEvent>
{
    private readonly ILogger<ReservationCreatedDomainEventHandler> _logger;

    public ReservationCreatedDomainEventHandler(ILogger<ReservationCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ReservationCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ReservationCreatedDomainEvent)} received. If reservation creation would have any side effect the imlementation would goes here.");

        return Task.CompletedTask;
    }
}
