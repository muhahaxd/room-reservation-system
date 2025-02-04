using Microsoft.Extensions.DependencyInjection;
using RRS.App.Application.Configuration;
using RRS.App.Presentation.CliCommands;

namespace RRS.App.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());

        services.AddScoped<CreateMeetingRoomCliCommandHandler>();
        services.AddScoped<CreateReservationCliCommandHandler>();
        services.AddScoped<DeleteMeetingRoomCliCommandHandler>();
        services.AddScoped<DeleteReservationCliCommandHandler>();
        services.AddScoped<ListMeetingRoomAvailabilityCliCommandHandler>();
        services.AddScoped<ListMeetingRoomsCliCommandHandler>();
        services.AddScoped<ListUpcomingReservationsCliCommandHandler>();
        services.AddScoped<UpdateReservationCliCommandHandler>();

        return services;
    }

    public static IServiceCollection AddApplicationOptions(this IServiceCollection services)
    {
        services.AddOptions<ReservationOptions>()
            .BindConfiguration(ReservationOptions.SectionName);

        return services;
    }
}
