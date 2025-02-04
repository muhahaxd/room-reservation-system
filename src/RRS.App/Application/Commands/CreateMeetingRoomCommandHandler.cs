using MediatR;
using Microsoft.Extensions.Logging;
using RRS.Infrastructure.Persistance.Transaction;
using RSS.Domain.Entities.MeetingRoom;

namespace RRS.App.Application.Commands;

internal record CreateMeetingRoomCommand(string Name, byte Capacity, bool OnlyWeekdays) : IRequest;

internal class CreateMeetingRoomCommandHandler : IRequestHandler<CreateMeetingRoomCommand>
{
    private readonly ILogger<CreateMeetingRoomCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMeetingRoomCommandHandler(ILogger<CreateMeetingRoomCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateMeetingRoomCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(CreateMeetingRoomCommand)} command received.");

        var count = await _unitOfWork.MeetingRoomRepository.CountAsync(entity =>
            entity.Name.ToLower().Equals(request.Name.ToLower())
        );

        if (count != 0)
        {
            _logger.LogError("Another meeting room already has the given name.");

            return;
        }

        var meetingRoom = new MeetingRoomEntity(request.Name, request.Capacity, request.OnlyWeekdays);

        await _unitOfWork.MeetingRoomRepository.InsertAsync(meetingRoom, cancellationToken);

        await _unitOfWork.SubmitChangesAsync(cancellationToken);

        _logger.LogInformation("Meeting room successfully created.");
    }
}
