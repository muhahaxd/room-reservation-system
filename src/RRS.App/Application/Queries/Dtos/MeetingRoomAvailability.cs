namespace RRS.App.Application.Queries.Dtos;
internal record MeetingRoomAvailability(Guid? Id, TimeOnly From, TimeOnly To, bool IsAvailable);
