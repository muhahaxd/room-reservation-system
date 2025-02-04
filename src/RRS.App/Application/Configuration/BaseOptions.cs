namespace RRS.App.Application.Configuration;
internal class BaseOptions<TOption>
{
    public static string SectionName => typeof(TOption).Name;
}
