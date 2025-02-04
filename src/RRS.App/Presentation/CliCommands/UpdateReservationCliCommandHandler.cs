using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using RRS.App.Application.Commands;
using RRS.App.Extensions;
using RRS.Infrastructure.Persistance.Repositories.MeetingRoom;

namespace RRS.App.Presentation.CliCommands;
internal class UpdateReservationCliCommandHandler : ICliCommandHandler
{
    public const string MeetingRoomNameKey = "name";
    public const string ReservationIdKey = "id";
    public const string StartKey = "start";
    public const string EndKey = "end";

    private readonly ILogger<CreateMeetingRoomCliCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IValidator<UpdateReservationCommand> _validator;
    private readonly IReadOnlyMeetingRoomRepository _readOnlyMeetingRoomRepository;

    public UpdateReservationCliCommandHandler(ILogger<CreateMeetingRoomCliCommandHandler> logger, IMediator mediator, IValidator<UpdateReservationCommand> validator, IReadOnlyMeetingRoomRepository readOnlyMeetingRoomRepository)
    {
        _logger = logger;
        _mediator = mediator;
        _validator = validator;
        _readOnlyMeetingRoomRepository = readOnlyMeetingRoomRepository;
    }

    public async Task Handle(Dictionary<string, object?> options)
    {
        _logger.LogDebug($"{nameof(UpdateReservationCliCommandHandler)} cli command received.");

        var meetingRoomName = (string)options[MeetingRoomNameKey]!;
        var meetingRoomId = await _readOnlyMeetingRoomRepository.FindMeetingRoomByNameAsync(meetingRoomName);

        if (meetingRoomId is null)
        {
            Console.WriteLine($"{meetingRoomName} meeting room does not exists.");

            return;
        }

        var updateReservationCommand = new UpdateReservationCommand((Guid)options[ReservationIdKey]!, meetingRoomId.Value, Environment.UserName, (DateTime)options[StartKey]!, (DateTime)options[EndKey]!);

        var result = await _validator.ValidateAsync(updateReservationCommand);
        if (!result.IsValid)
        {
            Console.WriteLine($"Parameters appears to be invalid due to the following reason(s): {result.GetErrorMessages()}");

            return;
        }

        await _mediator.Send(updateReservationCommand);

        Console.WriteLine("Reservation successfully updated.");
    }
}
