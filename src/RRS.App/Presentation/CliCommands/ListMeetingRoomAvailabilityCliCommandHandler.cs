using ConsoleTables;
using MediatR;
using Microsoft.Extensions.Logging;
using RRS.App.Application.Queries;
using RRS.Infrastructure.Persistance.Repositories.MeetingRoom;

namespace RRS.App.Presentation.CliCommands;
internal class ListMeetingRoomAvailabilityCliCommandHandler : ICliCommandHandler
{
    public const string MeetingRoomNameKey = "name";
    public const string DateKey = "date";

    private readonly IMediator _mediator;
    private readonly IReadOnlyMeetingRoomRepository _readOnlyMeetingRoomRepository;
    private readonly ILogger<ListMeetingRoomAvailabilityCliCommandHandler> _logger;

    public ListMeetingRoomAvailabilityCliCommandHandler(ILogger<ListMeetingRoomAvailabilityCliCommandHandler> logger, IMediator mediator, IReadOnlyMeetingRoomRepository readOnlyMeetingRoomRepository)
    {
        _logger = logger;
        _mediator = mediator;
        _readOnlyMeetingRoomRepository = readOnlyMeetingRoomRepository;
    }

    public async Task Handle(Dictionary<string, object?> options)
    {
        _logger.LogDebug($"{nameof(ListMeetingRoomAvailabilityCliCommandHandler)} cli command received.");

        var meetingRoomName = (string)options[MeetingRoomNameKey]!;
        var meetingRoomId = await _readOnlyMeetingRoomRepository.FindMeetingRoomByNameAsync(meetingRoomName);

        if (meetingRoomId is null)
        {
            Console.WriteLine($"{meetingRoomName} meeting room does not exists.");

            return;
        }

        options.TryGetValue(DateKey, out var date);

        var requestMeetingRoomsAvailabilityQuery = new RequestMeetingRoomsAvailabilityQuery(meetingRoomId.Value, date is null ? null : (DateOnly)date);

        var response = await _mediator.Send(requestMeetingRoomsAvailabilityQuery);

        ConsoleTable.From(response).Write();
    }
}
