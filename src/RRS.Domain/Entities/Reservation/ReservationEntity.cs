using RRS.Domain.Events;

namespace RSS.Domain.Entities.Reservation;
public class ReservationEntity : AggregateRoot
{
    // Only for EFCore Migration generation
    internal ReservationEntity() { }
    public ReservationEntity(string reservedBy, Guid meetingRoomId, DateTime from, DateTime to) : base()
    {
        MeetingRoomId = meetingRoomId;
        ReservedBy = reservedBy;
        ReservedFrom = from;
        ReservedUntil = to;
        ReservationLength = to - from;

        _domainEvents.Add(new ReservationCreatedDomainEvent(this));
    }

    public override void Delete()
    {
        base.Delete();

        _domainEvents.Add(new ReservationDeletedDomainEvent());
    }

    public void Update(DateTime newStart, DateTime newEnd)
    {
        ReservedFrom = newStart;
        ReservedUntil = newEnd;

        EntityChanged();

        _domainEvents.Add(new ReservationModifiedDomainEvent());
    }
}
