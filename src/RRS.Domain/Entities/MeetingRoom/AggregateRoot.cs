using RSS.Domain.Entities.Reservation;
using System.ComponentModel.DataAnnotations;

namespace RSS.Domain.Entities.MeetingRoom;

public class AggregateRoot : Entity
{
    public AggregateRoot() : base()
    {

    }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    [Range(1, 16)]
    public byte Capacity { get; set; }

    public bool OnlyWeekdays { get; set; }

    public HashSet<ReservationEntity> Reservations { get; set; }
}
