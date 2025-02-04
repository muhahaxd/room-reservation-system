using RSS.Domain.Entities.MeetingRoom;

namespace RRS.Infrastructure.Persistance.Repositories.MeetingRoom;
public class MeetingRoomRepository : Repository<MeetingRoomEntity>, IMeetingRoomRepository
{
    public MeetingRoomRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }
}