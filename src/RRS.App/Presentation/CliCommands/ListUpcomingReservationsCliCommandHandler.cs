using ConsoleTables;
using MediatR;
using Microsoft.Extensions.Logging;
using RRS.App.Application.Queries;
using RRS.Infrastructure.Persistance.Repositories.MeetingRoom;

namespace RRS.App.Presentation.CliCommands;
internal class ListUpcomingReservationsCliCommandHandler : ICliCommandHandler
{
    public const string MeetingRoomNameKey = "name";

    private readonly IMediator _mediator;
    private readonly IReadOnlyMeetingRoomRepository _readOnlyMeetingRoomRepository;
    private readonly ILogger<CreateMeetingRoomCliCommandHandler> _logger;

    public ListUpcomingReservationsCliCommandHandler(ILogger<CreateMeetingRoomCliCommandHandler> logger, IMediator mediator, IReadOnlyMeetingRoomRepository readOnlyMeetingRoomRepository)
    {
        _logger = logger;
        _mediator = mediator;
        _readOnlyMeetingRoomRepository = readOnlyMeetingRoomRepository;
    }

    public async Task Handle(Dictionary<string, object?> options)
    {
        _logger.LogDebug($"{nameof(ListUpcomingReservationsCliCommandHandler)} cli command received.");

        var meetingRoomName = (string)options[MeetingRoomNameKey]!;
        var meetingRoomId = await _readOnlyMeetingRoomRepository.FindMeetingRoomByNameAsync(meetingRoomName);

        if (meetingRoomId is null)
        {
            Console.WriteLine($"{meetingRoomName} meeting room does not exists.");

            return;
        }

        var requestUpcomingReservationsQuery = new RequestUpcomingReservationsQuery(meetingRoomId.Value);

        var response = await _mediator.Send(requestUpcomingReservationsQuery);

        ConsoleTable.From(response).Write();
    }
}
