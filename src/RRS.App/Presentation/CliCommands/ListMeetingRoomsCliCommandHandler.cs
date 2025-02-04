using ConsoleTables;
using MediatR;
using Microsoft.Extensions.Logging;
using RRS.App.Application.Queries;

namespace RRS.App.Presentation.CliCommands;
internal class ListMeetingRoomsCliCommandHandler : ICliCommandHandler
{
    private readonly IMediator _mediator;
    private readonly ILogger<CreateMeetingRoomCliCommandHandler> _logger;

    public ListMeetingRoomsCliCommandHandler(ILogger<CreateMeetingRoomCliCommandHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(Dictionary<string, object?> options)
    {
        _logger.LogDebug($"{nameof(ListMeetingRoomsCliCommandHandler)} cli command received.");
        var requestMeetingRoomsQuery = new RequestMeetingRoomsQuery();

        var response = await _mediator.Send(requestMeetingRoomsQuery);

        ConsoleTable.From(response).Write();
    }
}
