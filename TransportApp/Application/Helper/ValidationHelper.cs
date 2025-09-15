using FluentValidation;

namespace TransportApp.Application.Helper;

public static class ValidationHelper
{
    public static async Task<IResult?> ValidateAndResult<T>(T request, IValidator<T> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
            return Results.BadRequest(new { Errors = errors });
        }
        return null;
    }
}