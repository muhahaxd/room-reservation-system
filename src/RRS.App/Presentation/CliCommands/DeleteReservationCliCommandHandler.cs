using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using RRS.App.Application.Commands;
using RRS.App.Extensions;

namespace RRS.App.Presentation.CliCommands;
internal class DeleteReservationCliCommandHandler : ICliCommandHandler
{
    public const string ReservationIdKey = "id";

    private readonly ILogger<CreateMeetingRoomCliCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IValidator<DeleteReservationCommand> _validator;

    public DeleteReservationCliCommandHandler(ILogger<CreateMeetingRoomCliCommandHandler> logger, IMediator mediator, IValidator<DeleteReservationCommand> validator)
    {
        _logger = logger;
        _mediator = mediator;
        _validator = validator;
    }

    public async Task Handle(Dictionary<string, object?> options)
    {
        _logger.LogDebug($"{nameof(DeleteReservationCliCommandHandler)} cli command received.");

        var deleteReservationCommand = new DeleteReservationCommand((Guid)options[ReservationIdKey]!, Environment.UserName);

        var result = await _validator.ValidateAsync(deleteReservationCommand);
        if (!result.IsValid)
        {
            Console.WriteLine($"Parameters appears to be invalid due to the following reason(s): {result.GetErrorMessages()}");

            return;
        }

        await _mediator.Send(deleteReservationCommand);

        Console.WriteLine("Reservation successfully deleted.");
    }
}
