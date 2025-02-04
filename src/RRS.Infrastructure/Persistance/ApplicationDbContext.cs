using Microsoft.EntityFrameworkCore;

namespace RRS.Infrastructure.Persistance;
public class ApplicationDbContext : ReadOnlyApplicationDbContext
{
    public ApplicationDbContext() : base(QueryTrackingBehavior.TrackAll)
    {

    }
}
