namespace RRS.App.Extensions;
internal static class DateAndTimeExtensions
{
    public static TimeOnly ToTimeOnly(this DateTime dateTime) => TimeOnly.FromDateTime(dateTime);
}
