using FluentValidation;
using RRS.App.Application.Commands;
using RRS.Infrastructure.Persistance.Repositories.MeetingRoom;

namespace RRS.App.Application.Validators;
internal class DeleteMeetingRoomCommandValidator : AbstractValidator<DeleteMeetingRoomCommand>
{
    private readonly IReadOnlyMeetingRoomRepository _readOnlyMeetingRoomRepository;

    public DeleteMeetingRoomCommandValidator(IReadOnlyMeetingRoomRepository readOnlyMeetingRoomRepository)
    {
        _readOnlyMeetingRoomRepository = readOnlyMeetingRoomRepository;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(a => a.MeetingRoomId)
            .MustAsync(MeetingRoomExistsAsync)
            .WithMessage(a => $"Meeting room was not found with id: {a.MeetingRoomId}.");
    }

    private async Task<bool> MeetingRoomExistsAsync(Guid meetingRoomId, CancellationToken cancellationToken)
    {
        return await _readOnlyMeetingRoomRepository.AnyAsync(a => a.Id.Equals(meetingRoomId), cancellationToken);
    }
}
