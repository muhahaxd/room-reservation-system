using MediatR;
using Microsoft.Extensions.Logging;
using RRS.Infrastructure.Persistance.Transaction;

namespace RRS.App.Application.Commands;

internal record UpdateReservationCommand(Guid ReservationId, Guid MeetingRoomId, string UserName, DateTime Start, DateTime End)
    : CreateReservationCommand(MeetingRoomId, UserName, Start, End), IRequest;

internal class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand>
{
    private readonly ILogger<UpdateReservationCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReservationCommandHandler(ILogger<UpdateReservationCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(UpdateReservationCommand)} command received.");

        var reservation = await _unitOfWork.ReservationRepository.FirstOrDefaultAsync(a => a.Id.Equals(request.ReservationId),
            cancellationToken);

        if (reservation == null)
        {
            _logger.LogError("Reservation was not found with id {}.", request.ReservationId);

            return;
        }

        reservation.Update(request.Start, request.End);

        await _unitOfWork.SubmitChangesAsync(cancellationToken);

        _logger.LogInformation("Reservation successfully deleted.");
    }
}
