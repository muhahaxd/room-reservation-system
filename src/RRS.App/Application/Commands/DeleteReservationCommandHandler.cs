using MediatR;
using Microsoft.Extensions.Logging;
using RRS.Infrastructure.Persistance.Transaction;

namespace RRS.App.Application.Commands;

internal record DeleteReservationCommand(Guid ReservationId, string UserName) : IRequest;

internal class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand>
{
    private readonly ILogger<DeleteReservationCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReservationCommandHandler(ILogger<DeleteReservationCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(DeleteReservationCommand)} command received.");

        var reservation = await _unitOfWork.ReservationRepository.FirstOrDefaultAsync(a => a.Id.Equals(request.ReservationId) && a.ReservedBy.Equals(request.UserName),
            cancellationToken);

        if (reservation == null)
        {
            _logger.LogError("Reservation was not found with id {}.", request.ReservationId);

            return;
        }

        reservation.Delete();

        await _unitOfWork.SubmitChangesAsync(cancellationToken);

        _logger.LogInformation("Reservation successfully deleted.");
    }
}
