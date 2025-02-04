using Microsoft.EntityFrameworkCore;
using RSS.Domain.Entities.MeetingRoom;

namespace RRS.Infrastructure.Persistance.Repositories.MeetingRoom;
public class ReadOnlyMeetingRoomRepository : ReadOnlyRepository<MeetingRoomEntity>, IReadOnlyMeetingRoomRepository
{
    public ReadOnlyMeetingRoomRepository(ReadOnlyApplicationDbContext readOnlyApplicationDbContext) : base(readOnlyApplicationDbContext)
    {
    }

    public async Task<Guid?> FindMeetingRoomByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var id = await EntityNotDeleted().Where(meetingRoom => meetingRoom.Name.ToLower().Equals(name.Trim().ToLower()))
            .Select(meetingRoom => meetingRoom.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (id == default)
        {
            return null;
        }

        return id;
    }
}