using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using RRS.App.Application.Commands;
using RRS.App.Extensions;
using RRS.Infrastructure.Persistance.Repositories.MeetingRoom;

namespace RRS.App.Presentation.CliCommands;
internal class DeleteMeetingRoomCliCommandHandler : ICliCommandHandler
{
    public const string NameKey = "name";

    private readonly ILogger<CreateMeetingRoomCliCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IValidator<DeleteMeetingRoomCommand> _validator;
    private readonly IReadOnlyMeetingRoomRepository _readOnlyMeetingRoomRepository;

    public DeleteMeetingRoomCliCommandHandler(ILogger<CreateMeetingRoomCliCommandHandler> logger, IMediator mediator, IValidator<DeleteMeetingRoomCommand> validator,
        IReadOnlyMeetingRoomRepository readOnlyMeetingRoomRepository)
    {
        _logger = logger;
        _mediator = mediator;
        _validator = validator;
        _readOnlyMeetingRoomRepository = readOnlyMeetingRoomRepository;
    }

    public async Task Handle(Dictionary<string, object?> options)
    {
        _logger.LogDebug($"{nameof(DeleteMeetingRoomCliCommandHandler)} cli command received.");

        var meetingRoomName = (string)options[NameKey]!;
        var meetingRoomId = await _readOnlyMeetingRoomRepository.FindMeetingRoomByNameAsync(meetingRoomName);

        if (meetingRoomId is null)
        {
            Console.WriteLine($"{meetingRoomName} meeting room does not exists.");

            return;
        }

        var deleteMeetingRoomCommand = new DeleteMeetingRoomCommand(meetingRoomId.Value);

        var result = await _validator.ValidateAsync(deleteMeetingRoomCommand);
        if (!result.IsValid)
        {
            Console.WriteLine($"Parameters appears to be invalid due to the following reason(s): {result.GetErrorMessages()}");

            return;
        }

        await _mediator.Send(deleteMeetingRoomCommand);

        Console.WriteLine("Meeting room successfully deleted.");
    }
}
