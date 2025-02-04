using FluentValidation;
using Microsoft.Extensions.Options;
using RRS.App.Application.Commands;
using RRS.App.Application.Configuration;
using RRS.Infrastructure.Persistance.Repositories.MeetingRoom;
using RRS.Infrastructure.Persistance.Repositories.Reservation;

namespace RRS.App.Application.Validators;
internal class UpdateReservationCommandValidator : AbstractValidator<UpdateReservationCommand>
{
    private readonly IReadOnlyMeetingRoomRepository _readOnlyMeetingRoomRepository;
    private readonly IReadOnlyReservationRepository _readOnlyReservationRepository;

    public UpdateReservationCommandValidator(IOptions<ReservationOptions> options,
        IReadOnlyMeetingRoomRepository readOnlyMeetingRoomRepository,
        IReadOnlyReservationRepository readOnlyReservationRepository)
    {
        _readOnlyMeetingRoomRepository = readOnlyMeetingRoomRepository;
        _readOnlyReservationRepository = readOnlyReservationRepository;

        RuleLevelCascadeMode = CascadeMode.Stop;

        Include(new CreateReservationCommandValidator(options, readOnlyMeetingRoomRepository, readOnlyReservationRepository));

        RuleFor(a => a)
            .MustAsync(ReservationExistsAsync)
            .WithMessage(a => $"Reservation with id {a.ReservationId} does not exists or not belongs to the user: {a.UserName}.");
    }

    private async Task<bool> ReservationExistsAsync(UpdateReservationCommand updateReservationCommand, CancellationToken cancellationToken)
    {
        return await _readOnlyReservationRepository.AnyAsync(a =>
            a.Id.Equals(updateReservationCommand.ReservationId)
            && a.ReservedBy.Equals(updateReservationCommand.UserName)
        , cancellationToken);
    }
}
