using FluentValidation;
using Microsoft.Extensions.Options;
using RRS.App.Application.Commands;
using RRS.App.Application.Configuration;
using RRS.App.Extensions;
using RRS.Infrastructure.Persistance.Repositories.MeetingRoom;
using RRS.Infrastructure.Persistance.Repositories.Reservation;

namespace RRS.App.Application.Validators;
internal class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    private readonly ReservationOptions _options;
    private readonly IReadOnlyMeetingRoomRepository _readOnlyMeetingRoomRepository;
    private readonly IReadOnlyReservationRepository _readOnlyReservationRepository;

    public CreateReservationCommandValidator(IOptions<ReservationOptions> options,
        IReadOnlyMeetingRoomRepository readOnlyMeetingRoomRepository,
        IReadOnlyReservationRepository readOnlyReservationRepository)
    {
        _options = options.Value;
        _readOnlyMeetingRoomRepository = readOnlyMeetingRoomRepository;
        _readOnlyReservationRepository = readOnlyReservationRepository;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(a => a)
            .MustAsync(MeetingRoomExistsAsync)
            .WithMessage(a => $"Meeting room was not found with id {a.MeetingRoomId} or not available between {a.Start} and {a.End}.");

        RuleFor(a => a)
            .MustAsync(NoConflictedReservationAsync)
            .WithMessage(a => $"The given time range from {a.Start} to {a.End} conflicts with another reservation.");

        RuleFor(a => a)
            .Must(a => a.Start.Date.Equals(a.End.Date))
            .WithMessage("Start and End date must be on the same day.");

        RuleFor(a => a)
            .Must(a => a.Start <= a.End)
            .WithMessage("End date must be greater then the start.");

        RuleFor(a => a.End)
            .Must(a => a.ToTimeOnly() <= _options.TimeOfWorkEnd)
            .WithMessage($"Invalid reservation time. The given time must be between {_options.TimeOfWorkStart} and {_options.TimeOfWorkEnd}.");

        RuleFor(a => a.Start)
            .Must(a => a.ToTimeOnly() >= _options.TimeOfWorkStart)
            .WithMessage($"Invalid reservation time. The given time must be between {_options.TimeOfWorkStart} and {_options.TimeOfWorkEnd}.");
    }

    private async Task<bool> MeetingRoomExistsAsync(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var isWeekend = (request.Start.DayOfWeek | request.End.DayOfWeek).HasFlag(DayOfWeek.Saturday | DayOfWeek.Sunday);

        return await _readOnlyMeetingRoomRepository.AnyAsync(a => a.Id.Equals(request.MeetingRoomId) && a.OnlyWeekdays.Equals(!isWeekend), cancellationToken);
    }

    private async Task<bool> NoConflictedReservationAsync(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var conflictedReservation = await _readOnlyReservationRepository.FirstOrDefaultAsync(entity =>
                (request.Start >= entity.ReservedFrom && request.Start <= entity.ReservedUntil)
                || (request.End >= entity.ReservedFrom && request.End <= entity.ReservedUntil)
            , cancellationToken);

        return conflictedReservation == null;
    }
}
