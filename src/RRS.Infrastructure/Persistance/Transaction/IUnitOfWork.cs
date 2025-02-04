using RRS.Infrastructure.Persistance.Repositories.MeetingRoom;
using RRS.Infrastructure.Persistance.Repositories.Reservation;

namespace RRS.Infrastructure.Persistance.Transaction;
public interface IUnitOfWork : IDisposable
{
    IMeetingRoomRepository MeetingRoomRepository { get; }
    IReservationRepository ReservationRepository { get; }

    Task SubmitChangesAsync(CancellationToken cancellationToken = default);
}
