using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace GM.Exceptions.Sample.API.Controllers;

/// <summary>
/// Each action throws a GM.Exceptions type; the <c>CustomExceptionHandler</c> maps it to an HTTP
/// status + ProblemDetails, with a message localized from Resources/GMExceptionsResource.resx.
/// </summary>
[ApiController]
[Route("[controller]")]
public class SamplesController : ControllerBase
{
    /// <summary>404 Not Found — "Sample with property Id: 1 not found".</summary>
    [HttpGet("not-found")]
    public IActionResult NotFoundSample() => throw new NotFoundException("Sample", "Id", 1);

    /// <summary>409 Conflict — "Sample with property Name: demo already exists".</summary>
    [HttpGet("already-exists")]
    public IActionResult AlreadyExists() => throw new AlreadyExistsException("Sample", "Name", "demo");

    /// <summary>400 Bad Request.</summary>
    [HttpGet("bad-request")]
    public IActionResult BadRequestSample() => throw new BadRequestException("The request was invalid.");

    /// <summary>400 Bad Request with a "restricted to delete" message.</summary>
    [HttpGet("delete-restricted")]
    public IActionResult DeleteRestricted() => throw new DeleteException("Sample", "Id", 1);

    /// <summary>400 Bad Request with per-field validation errors.</summary>
    [HttpGet("validation")]
    public IActionResult Validation() => throw new ValidationException(
    [
        new ValidationFailure("Name", "'Name' must not be empty."),
        new ValidationFailure("Email", "'Email' is not a valid email address."),
    ]);
}
