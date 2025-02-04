using RSS.Domain.Entities.Reservation;

namespace RRS.Infrastructure.Persistance.Repositories.Reservation;
public class ReadOnlyReservationRepository : ReadOnlyRepository<ReservationEntity>, IReadOnlyReservationRepository
{
    public ReadOnlyReservationRepository(ReadOnlyApplicationDbContext readOnlyApplicationDbContext) : base(readOnlyApplicationDbContext)
    {
    }
}
