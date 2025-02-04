using MediatR;
using Microsoft.Extensions.Logging;
using RRS.App.Application.Queries.Dtos;
using RRS.App.Extensions;
using RRS.Infrastructure.Persistance.Repositories.Reservation;

namespace RRS.App.Application.Queries;
internal record RequestUpcomingReservationsQuery(Guid MeetingRoomId) : IRequest<IReadOnlyList<MeetingRoomAvailability>>;

internal class RequestUpcomingReservationsQueryHandler : IRequestHandler<RequestUpcomingReservationsQuery, IReadOnlyList<MeetingRoomAvailability>>
{
    private readonly ILogger<RequestUpcomingReservationsQueryHandler> _logger;
    private readonly IReadOnlyReservationRepository _readOnlyReservationRepository;

    public RequestUpcomingReservationsQueryHandler(ILogger<RequestUpcomingReservationsQueryHandler> logger, IReadOnlyReservationRepository readOnlyReservationRepository)
    {
        _logger = logger;
        _readOnlyReservationRepository = readOnlyReservationRepository;
    }

    public async Task<IReadOnlyList<MeetingRoomAvailability>> Handle(RequestUpcomingReservationsQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var tomorrowDate = now.Date;

        var upcomingReservations = await _readOnlyReservationRepository.ListAsync(a =>
            a.MeetingRoomId.Equals(request.MeetingRoomId)
            && a.ReservedFrom.Date.Equals(tomorrowDate)
            && a.ReservedFrom > now, cancellationToken);

        return upcomingReservations.Select(s => new MeetingRoomAvailability(s.Id, s.ReservedFrom.ToTimeOnly(), s.ReservedUntil.ToTimeOnly(), IsAvailable: false)).ToList();
    }
}