using RSS.Domain.Entities.Reservation;

namespace RRS.Infrastructure.Persistance.Repositories.Reservation;
public interface IReadOnlyReservationRepository : IReadOnlyRepository<ReservationEntity>
{
}
