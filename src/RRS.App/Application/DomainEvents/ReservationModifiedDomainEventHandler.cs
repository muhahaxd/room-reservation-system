using MediatR;
using Microsoft.Extensions.Logging;
using RRS.Domain.Events;

namespace RRS.App.Application.DomainEvents;
public class ReservationModifiedDomainEventHandler : INotificationHandler<ReservationModifiedDomainEvent>
{
    private readonly ILogger<ReservationModifiedDomainEventHandler> _logger;

    public ReservationModifiedDomainEventHandler(ILogger<ReservationModifiedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ReservationModifiedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ReservationModifiedDomainEvent)} received. If reservation modification would have any side effect the imlementation would goes here.");

        return Task.CompletedTask;
    }
}
