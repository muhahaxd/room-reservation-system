using RSS.Domain.Entities.Reservation;

namespace RRS.Infrastructure.Persistance.Repositories.Reservation;
public class ReservationRepository : Repository<ReservationEntity>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }
}
