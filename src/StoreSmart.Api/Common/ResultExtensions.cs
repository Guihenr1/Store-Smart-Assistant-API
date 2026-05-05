using Ardalis.Result;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace StoreSmart.Api.Common;

public static class ResultExtensions
{
    public static IResult ToMinimalApiResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        // Validation Errors
        if (result.Status == ResultStatus.Invalid)
        {
            var validationErrors = result.ValidationErrors?
                .GroupBy(e => e.Identifier.ToLowerInvariant())
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            return Results.BadRequest(new
            {
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Errors = validationErrors ?? new Dictionary<string, string[]>()
            });
        }

        // Other error types
        var errorList = result.Errors?.ToArray() ?? Array.Empty<string>();

        return result.Status switch
        {
            ResultStatus.Conflict => Results.Conflict(errorList),
            ResultStatus.NotFound => Results.NotFound(errorList),
            ResultStatus.Unauthorized => Results.Unauthorized(),
            ResultStatus.Forbidden => Results.Forbid(),
            ResultStatus.Error => Results.Problem(
                title: "An error occurred",
                detail: errorList.FirstOrDefault() ?? "Internal Server Error",
                statusCode: StatusCodes.Status500InternalServerError),

            _ => Results.BadRequest(errorList)
        };
    }

    // Overload for non-generic Result
    public static IResult ToMinimalApiResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok();
        }

        if (result.Status == ResultStatus.Invalid)
        {
            var validationErrors = result.ValidationErrors?
                .GroupBy(e => e.Identifier.ToLowerInvariant())
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            return Results.BadRequest(new
            {
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Errors = validationErrors ?? new Dictionary<string, string[]>()
            });
        }

        var errorList = result.Errors?.ToArray() ?? Array.Empty<string>();

        return result.Status switch
        {
            ResultStatus.Conflict => Results.Conflict(errorList),
            ResultStatus.NotFound => Results.NotFound(errorList),
            ResultStatus.Unauthorized => Results.Unauthorized(),
            ResultStatus.Forbidden => Results.Forbid(),
            ResultStatus.Error => Results.Problem(
                title: "An error occurred",
                detail: errorList.FirstOrDefault(),
                statusCode: StatusCodes.Status500InternalServerError),

            _ => Results.BadRequest(errorList)
        };
    }
}