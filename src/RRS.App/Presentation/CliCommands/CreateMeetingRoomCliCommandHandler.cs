using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using RRS.App.Application.Commands;
using RRS.App.Extensions;

namespace RRS.App.Presentation.CliCommands;
internal class CreateMeetingRoomCliCommandHandler : ICliCommandHandler
{
    public const string NameKey = "name";
    public const string CapacityKey = "capacity";
    public const string OnlyWeekendsKey = "only-weekdays";

    private readonly ILogger<CreateMeetingRoomCliCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IValidator<CreateMeetingRoomCommand> _validator;

    public CreateMeetingRoomCliCommandHandler(ILogger<CreateMeetingRoomCliCommandHandler> logger, IMediator mediator, IValidator<CreateMeetingRoomCommand> validator)
    {
        _logger = logger;
        _mediator = mediator;
        _validator = validator;
    }

    public async Task Handle(Dictionary<string, object?> options)
    {
        _logger.LogDebug($"{nameof(CreateMeetingRoomCliCommandHandler)} cli command received.");

        // I know unnecessary boxing and unboxing can cause performance issues, however, I didn't want to write 
        // my own CLI framework and this was the best quick solution to make it generic
        var createMeetingRoomCommand = new CreateMeetingRoomCommand((string)options[NameKey]!, (byte)options[CapacityKey]!, (bool)options[OnlyWeekendsKey]!);

        var result = await _validator.ValidateAsync(createMeetingRoomCommand);
        if (!result.IsValid)
        {
            Console.WriteLine($"Parameters appears to be invalid due to the following reason(s): {result.GetErrorMessages()}");

            return;
        }

        await _mediator.Send(createMeetingRoomCommand);

        Console.WriteLine("Meeting room successfully created.");
    }
}
