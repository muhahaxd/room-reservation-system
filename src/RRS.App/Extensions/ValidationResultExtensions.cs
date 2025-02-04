using FluentValidation.Results;

namespace RRS.App.Extensions;
internal static class ValidationResultExtensions
{
    public static string GetErrorMessages(this ValidationResult result)
    {
        if (result.IsValid)
        {
            throw new InvalidOperationException();
        }

        return string.Join(',', result.Errors.Select(s => s.ErrorMessage));
    }

}
