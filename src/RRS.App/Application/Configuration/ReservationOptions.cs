namespace RRS.App.Application.Configuration;
internal class ReservationOptions : BaseOptions<ReservationOptions>
{
    public TimeOnly TimeOfWorkStart { get; set; }
    public TimeOnly TimeOfWorkEnd { get; set; }
}