using MediatR;
using Microsoft.Extensions.Logging;
using RRS.Infrastructure.Persistance.Transaction;

namespace RRS.App.Application.Commands;

internal record DeleteMeetingRoomCommand(Guid MeetingRoomId) : IRequest;

internal class DeleteMeetingRoomCommandHandler : IRequestHandler<DeleteMeetingRoomCommand>
{
    private readonly ILogger<DeleteMeetingRoomCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMeetingRoomCommandHandler(ILogger<DeleteMeetingRoomCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteMeetingRoomCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(DeleteMeetingRoomCommand)} command received.");

        var meetingRoom = await _unitOfWork.MeetingRoomRepository.FirstOrDefaultAsync(a => a.Id.Equals(request.MeetingRoomId), cancellationToken);

        if (meetingRoom == null)
        {
            _logger.LogError("Meeting room with id {MeetingRoomId} was not found.", request.MeetingRoomId);

            return;
        }

        meetingRoom!.Delete();

        await _unitOfWork.SubmitChangesAsync(cancellationToken);

        _logger.LogInformation("Meeting room successfully deleted.");
    }
}
