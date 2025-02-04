using FluentValidation;
using RRS.App.Application.Commands;
using RRS.Infrastructure.Persistance.Repositories.Reservation;

namespace RRS.App.Application.Validators;
internal class DeleteReservationCommandValidator : AbstractValidator<DeleteReservationCommand>
{
    private readonly IReadOnlyReservationRepository _readOnlyReservationRepository;

    public DeleteReservationCommandValidator(IReadOnlyReservationRepository readOnlyReservationRepository)
    {
        _readOnlyReservationRepository = readOnlyReservationRepository;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(a => a)
            .MustAsync(ReservationMustExists)
            .WithMessage(a => $"Reservation was not found with id: {a.ReservationId} or not related to the given user {a.UserName}..");
    }

    private async Task<bool> ReservationMustExists(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        return await _readOnlyReservationRepository.AnyAsync(a => a.Id.Equals(request.ReservationId) && a.ReservedBy.Equals(request.UserName), cancellationToken);
    }
}
