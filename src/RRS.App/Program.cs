using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RRS.App;
using RRS.App.Presentation;
using RRS.App.Presentation.CliCommands;
using RRS.Infrastructure.Persistance;
using System.CommandLine;
using System.CommandLine.Parsing;

var services = new Startup().ConfigureServices().Build();

using (var scope = services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
}

var rootCommand = new CliBuilder(services)
    .CreateCommand("rooms", "Command to manage meeting rooms.")
        .CreateSubCommand("create", "Creates a new instance.")
            .AddOption<string>($"--{CreateMeetingRoomCliCommandHandler.NameKey}", "Name of the meeting room")
            .AddOption<byte>($"--{CreateMeetingRoomCliCommandHandler.CapacityKey}", "Capacity of the meeting room")
            .AddOption<bool>($"--{CreateMeetingRoomCliCommandHandler.OnlyWeekendsKey}", "Indicates whether the meeting room is open at the weekends or not.")
            .SetHandler<CreateMeetingRoomCliCommandHandler>()
            .BuildSubcommand()
        .CreateSubCommand("delete", "Deletes an existing meeting room.")
            .AddOption<string>($"--{DeleteMeetingRoomCliCommandHandler.NameKey}", "Name of the meeting room.")
            .SetHandler<DeleteMeetingRoomCliCommandHandler>()
            .BuildSubcommand()
        .CreateSubCommand("list", "Lists all metting rooms.")
            .SetHandler<ListMeetingRoomsCliCommandHandler>()
            .BuildSubcommand()
        .CreateSubCommand("availability", "Calculates the room availability for a certain date.")
            .AddOption<string>($"--{ListMeetingRoomAvailabilityCliCommandHandler.MeetingRoomNameKey}", "Name of the meeting room.")
            .AddOption<DateOnly>($"--{ListMeetingRoomAvailabilityCliCommandHandler.DateKey}", "Start time of the reservation.")
            .SetHandler<ListMeetingRoomAvailabilityCliCommandHandler>()
            .BuildSubcommand()
        .BuildCommand()
    .CreateCommand("reservation", "Command to manage reservations.")
        .CreateSubCommand("create", "Creates a new reservation")
            .AddOption<string>($"--{CreateReservationCliCommandHandler.MeetingRoomNameKey}", "Name of the meeting room.")
            .AddOption<DateTime>($"--{CreateReservationCliCommandHandler.StartKey}", "Start time of the reservation.")
            .AddOption<DateTime>($"--{CreateReservationCliCommandHandler.EndKey}", "Start time of the reservation.")
            .SetHandler<CreateReservationCliCommandHandler>()
            .BuildSubcommand()
        .CreateSubCommand("delete", "Deletes an existing reservation.")
            .AddOption<Guid>($"--{DeleteReservationCliCommandHandler.ReservationIdKey}", "Id of the reservation.")
            .SetHandler<DeleteReservationCliCommandHandler>()
            .BuildSubcommand()
        .CreateSubCommand("update", "Updates an existing reservation")
            .AddOption<string>($"--{UpdateReservationCliCommandHandler.MeetingRoomNameKey}", "Name of the meeting room.")
            .AddOption<string>($"--{UpdateReservationCliCommandHandler.ReservationIdKey}", "Id of the reservation.")
            .AddOption<DateTime>($"--{UpdateReservationCliCommandHandler.StartKey}", "Start time of the reservation.")
            .AddOption<DateTime>($"--{UpdateReservationCliCommandHandler.EndKey}", "Start time of the reservation.")
            .SetHandler<CreateReservationCliCommandHandler>()
            .BuildSubcommand()
        .CreateSubCommand("list", "Lists out the upcoming reservations for today.")
            .AddOption<string>($"--{UpdateReservationCliCommandHandler.MeetingRoomNameKey}", "Name of the meeting room.")
            .SetHandler<ListUpcomingReservationsCliCommandHandler>()
            .BuildSubcommand()
        .BuildCommand()
    .Build();

string? command = null;
while ((command = Console.ReadLine()) != "exit")
{
    //var commandArgs = CommandLineStringSplitter.Instance.Split(command);

    await rootCommand.InvokeAsync(command);
}