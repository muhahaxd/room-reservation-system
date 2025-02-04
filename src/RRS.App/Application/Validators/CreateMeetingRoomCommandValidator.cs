using FluentValidation;
using RRS.App.Application.Commands;
using RRS.Infrastructure.Persistance.Repositories.MeetingRoom;

namespace RRS.App.Application.Validators;
internal class CreateMeetingRoomCommandValidator : AbstractValidator<CreateMeetingRoomCommand>
{
    private readonly IReadOnlyMeetingRoomRepository _readOnlyMeetingRoomRepository;

    public CreateMeetingRoomCommandValidator(IReadOnlyMeetingRoomRepository readOnlyMeetingRoomRepository)
    {
        _readOnlyMeetingRoomRepository = readOnlyMeetingRoomRepository;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(a => a.Name)
            .MustAsync(MeetingRoomExistsAsync)
            .WithMessage(a => $"Meeting room already exists with name: {a.Name}.");
    }

    private async Task<bool> MeetingRoomExistsAsync(string name, CancellationToken cancellationToken)
    {
        var exists = await _readOnlyMeetingRoomRepository.AnyAsync(a => a.Name.ToLower().Equals(name.ToLower()), cancellationToken);

        return !exists;
    }
}
