using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RRS.App.Application.Configuration;
using RRS.App.Application.Queries.Dtos;
using RRS.App.Extensions;
using RRS.Infrastructure.Persistance.Repositories.Reservation;

namespace RRS.App.Application.Queries;

internal record RequestMeetingRoomsAvailabilityQuery(Guid MeetingRoomId, DateOnly? Date) : IRequest<IReadOnlyList<MeetingRoomAvailability>>;

internal class RequestMeetingRoomsAvailabilityQueryHandler : IRequestHandler<RequestMeetingRoomsAvailabilityQuery, IReadOnlyList<MeetingRoomAvailability>>
{
    private readonly ILogger<RequestMeetingRoomsAvailabilityQueryHandler> _logger;
    private readonly IReadOnlyReservationRepository _readOnlyReservationRepository;
    private readonly ReservationOptions _options;

    public RequestMeetingRoomsAvailabilityQueryHandler(ILogger<RequestMeetingRoomsAvailabilityQueryHandler> logger, IReadOnlyReservationRepository readOnlyReservationRepository, IOptions<ReservationOptions> options)
    {
        _logger = logger;
        _readOnlyReservationRepository = readOnlyReservationRepository;
        _options = options.Value;
    }

    public async Task<IReadOnlyList<MeetingRoomAvailability>> Handle(RequestMeetingRoomsAvailabilityQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var date = request.Date is null ? now.Date : new DateTime(request.Date.Value, TimeOnly.MinValue);

        var reservations = await _readOnlyReservationRepository.ListAsync(
            reservation => reservation.MeetingRoomId.Equals(request.MeetingRoomId) && reservation.ReservedFrom.Date.Equals(date),
            order => order.ReservedFrom,
            cancellationToken);

        var result = new List<MeetingRoomAvailability>();
        var nowTime = now.ToTimeOnly();
        var sameDate = date.Equals(now.Date);

        if (sameDate && nowTime > _options.TimeOfWorkEnd)
        {
            return result;
        }

        if (!reservations.Any())
        {
            result.Add(new(Id: null, _options.TimeOfWorkStart, _options.TimeOfWorkEnd, IsAvailable: true));

            return result;
        }

        var lastStart = sameDate && nowTime > _options.TimeOfWorkStart ? nowTime : _options.TimeOfWorkStart;
        var i = 0;
        while (lastStart != _options.TimeOfWorkEnd)
        {
            var reservation = i <= reservations.Count - 1 ? reservations[i] : null;

            if (reservation is null)
            {
                result.Add(new(Id: null, lastStart, _options.TimeOfWorkEnd, IsAvailable: true));

                lastStart = _options.TimeOfWorkEnd;
            }
            else
            {
                var reservedFrom = reservation.ReservedFrom.ToTimeOnly();
                if (reservedFrom > lastStart)
                {
                    result.Add(new(Id: null, lastStart, reservedFrom, IsAvailable: true));
                }

                var reservedUntil = reservation.ReservedUntil.ToTimeOnly();
                result.Add(new(reservation.Id, reservedFrom, reservedUntil, IsAvailable: false));

                lastStart = reservedUntil;
            }

            i++;
        }

        //foreach (var reservation in reservations)
        //{
        //    var reservedFrom = reservation.ReservedFrom.ToTimeOnly();
        //    if (reservedFrom > lastStart)
        //    {
        //        result.Add(new(Id: null, lastStart, reservedFrom, IsAvailable: true));
        //    }

        //    var reservedUntil = reservation.ReservedUntil.ToTimeOnly();
        //    result.Add(new(reservation.Id, reservedFrom, reservedUntil, IsAvailable: false));

        //    lastStart = reservedUntil;
        //}

        return result;
    }
}
