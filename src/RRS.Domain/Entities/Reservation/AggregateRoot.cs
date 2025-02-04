using RSS.Domain.Entities.MeetingRoom;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RSS.Domain.Entities.Reservation;
public class AggregateRoot : Entity
{
    public AggregateRoot() : base()
    {

    }

    [ForeignKey(nameof(MeetingRoom))]
    public Guid MeetingRoomId { get; set; }

    [Required, MaxLength(100)]
    public string ReservedBy { get; protected set; }

    [Required]
    public DateTime ReservedFrom { get; protected set; }

    [Required]
    public DateTime ReservedUntil { get; protected set; }

    [Required]
    public TimeSpan ReservationLength { get; protected set; }

    public MeetingRoomEntity MeetingRoom { get; protected set; }
}
