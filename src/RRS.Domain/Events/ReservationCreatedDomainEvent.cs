using MediatR;
using RSS.Domain.Entities.Reservation;

namespace RRS.Domain.Events;
public class ReservationCreatedDomainEvent : INotification
{
    private readonly ReservationEntity _createdReservation;

    public ReservationCreatedDomainEvent(ReservationEntity createdReservation)
    {
        _createdReservation = createdReservation;
    }
}
