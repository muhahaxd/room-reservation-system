using RSS.Domain.Entities.MeetingRoom;

namespace RRS.Infrastructure.Persistance.Repositories.MeetingRoom;
public interface IReadOnlyMeetingRoomRepository : IReadOnlyRepository<MeetingRoomEntity>
{
    Task<Guid?> FindMeetingRoomByNameAsync(string name, CancellationToken cancellationToken = default);
}
