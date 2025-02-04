using MediatR;
using Microsoft.Extensions.Logging;
using RRS.Infrastructure.Persistance.Transaction;
using RSS.Domain.Entities.Reservation;

namespace RRS.App.Application.Commands;

internal record CreateReservationCommand(Guid MeetingRoomId, string ReservedBy, DateTime Start, DateTime End) : IRequest;

internal class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand>
{
    private readonly ILogger<CreateReservationCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReservationCommandHandler(ILogger<CreateReservationCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(CreateReservationCommand)} command received.");

        var reservation = new ReservationEntity(request.ReservedBy, request.MeetingRoomId, request.Start, request.End);

        await _unitOfWork.ReservationRepository.InsertAsync(reservation, cancellationToken);

        await _unitOfWork.SubmitChangesAsync(cancellationToken);

        _logger.LogInformation($"Reservation successfully created from {request.Start} to {request.End}.");
    }
}
