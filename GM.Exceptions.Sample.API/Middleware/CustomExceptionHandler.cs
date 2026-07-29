using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GM.Exceptions.Sample.API.Middleware;

/// <summary>
/// Turns any <see cref="CustomException"/> from GM.Exceptions into an RFC 9457 ProblemDetails
/// response, mapping each exception type to an appropriate HTTP status code. The exception's
/// message is already localized (via the GM.Exceptions resource lookup + the project's .resx).
/// Non-GM exceptions are left for the next handler.
/// </summary>
public sealed class CustomExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not CustomException)
            return false;

        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            AlreadyExistsException => StatusCodes.Status409Conflict,
            BadRequestException => StatusCodes.Status400BadRequest,
            ValidationException => StatusCodes.Status400BadRequest,
            InternalServerException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest,
        };

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = "An error occurred.",
            Detail = exception.Message,
        };

        if (exception is ValidationException validationException)
            problemDetails.Extensions["errors"] = validationException.Failures;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception,
        });
    }
}
