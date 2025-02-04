namespace RSS.Domain.Entities.MeetingRoom;
public class MeetingRoomEntity : AggregateRoot
{
    public MeetingRoomEntity(string name, byte capacity, bool onlyWeekdays) : base()
    {
        Name = name;
        Capacity = capacity;
        OnlyWeekdays = onlyWeekdays;
    }
}
