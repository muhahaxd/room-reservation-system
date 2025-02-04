using Microsoft.Extensions.DependencyInjection;
using RRS.Infrastructure.Persistance;
using RRS.Infrastructure.Persistance.Repositories.MeetingRoom;
using RRS.Infrastructure.Persistance.Repositories.Reservation;
using RRS.Infrastructure.Persistance.Transaction;

namespace RRS.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>();
        services.AddDbContext<ReadOnlyApplicationDbContext>();

        services.AddSingleton<IReadOnlyMeetingRoomRepository, ReadOnlyMeetingRoomRepository>();
        services.AddSingleton<IReadOnlyReservationRepository, ReadOnlyReservationRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
