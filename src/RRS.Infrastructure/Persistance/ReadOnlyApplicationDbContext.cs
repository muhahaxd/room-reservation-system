using Microsoft.EntityFrameworkCore;
using RSS.Domain.Entities.MeetingRoom;
using RSS.Domain.Entities.Reservation;

namespace RRS.Infrastructure.Persistance;
public class ReadOnlyApplicationDbContext : DbContext
{
    private string _dbPath { get; set; }

    public ReadOnlyApplicationDbContext() : this(QueryTrackingBehavior.NoTracking)
    {
    }

    //dotnet ef migrations add InitialMigration --context ApplicationDbContext

    public ReadOnlyApplicationDbContext(QueryTrackingBehavior queryTrackingBehavior)
    {
        ChangeTracker.QueryTrackingBehavior = queryTrackingBehavior;
    }

    public DbSet<MeetingRoomEntity> MeetingRooms { get; set; }
    public DbSet<ReservationEntity> Reservations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        _dbPath = "rrs_database_lite.db";

        options.UseSqlite($"Data Source={_dbPath}");
    }
}
