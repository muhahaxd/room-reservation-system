using MediatR;
using Microsoft.Extensions.Logging;
using RRS.Infrastructure.Persistance.Repositories.MeetingRoom;
using RSS.Domain.Entities.MeetingRoom;

namespace RRS.App.Application.Queries;

internal record RequestMeetingRoomsQuery() : IRequest<IReadOnlyList<MeetingRoomEntity>>;

internal class RequestMeetingRoomsQueryHandler : IRequestHandler<RequestMeetingRoomsQuery, IReadOnlyList<MeetingRoomEntity>>
{
    private readonly ILogger<RequestMeetingRoomsQueryHandler> _logger;
    private readonly IReadOnlyMeetingRoomRepository _readOnlyMeetingRoomRepository;

    public RequestMeetingRoomsQueryHandler(ILogger<RequestMeetingRoomsQueryHandler> logger, IReadOnlyMeetingRoomRepository readOnlyMeetingRoomRepository)
    {
        _logger = logger;
        _readOnlyMeetingRoomRepository = readOnlyMeetingRoomRepository;
    }

    public async Task<IReadOnlyList<MeetingRoomEntity>> Handle(RequestMeetingRoomsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(RequestMeetingRoomsQueryHandler)} received.");

        return await _readOnlyMeetingRoomRepository.ListAsync(a => true, cancellationToken);
    }
}
