<p align="center">
  <img src="icon.png" alt="GM.Exceptions Samples" width="140" height="140" />
</p>

# GM.Exceptions Samples

[![CI](https://github.com/gmetskhvarishvili/GM.Exceptions.Samples/actions/workflows/ci.yml/badge.svg)](https://github.com/gmetskhvarishvili/GM.Exceptions.Samples/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

A small ASP.NET Core Web API showing how to use **[GM.Exceptions](https://www.nuget.org/packages/GM.Exceptions)**:
throw a typed exception, and a global exception handler turns it into a clean HTTP response —
with a **localized message** pulled from a `.resx`. Targets **.NET 10**.

## What it demonstrates

- **Typed exceptions → HTTP status codes.** `CustomExceptionHandler` (an `IExceptionHandler`)
  maps each GM.Exceptions type to a status and RFC 9457 `ProblemDetails`:

  | Thrown | Response |
  | --- | --- |
  | `NotFoundException` | `404 Not Found` |
  | `AlreadyExistsException` | `409 Conflict` |
  | `BadRequestException` / `DeleteException` | `400 Bad Request` |
  | `ValidationException` | `400` + per-field `errors` |
  | `InternalServerException` | `500` |

- **Localized messages.** `Resources/GMExceptionsResource.resx` provides the message templates
  (`NotFoundError` = `"{0} with property {1}: {2} not found"`, …). GM.Exceptions finds this
  resource automatically, so `new NotFoundException("Sample", "Id", 1)` responds with
  *"Sample with property Id: 1 not found"*. Add culture-specific `.resx` files for translations.

## Running

```bash
dotnet run --project GM.Exceptions.Sample.API
```

Then hit the endpoints (Swagger UI in Development), e.g.:

```bash
curl -k https://localhost:7182/api/v1/samples/not-found     # 404
curl -k https://localhost:7182/api/v1/samples/already-exists # 409
curl -k https://localhost:7182/api/v1/samples/validation     # 400 + errors
curl -k https://localhost:7182/health/live                   # liveness
curl -k https://localhost:7182/health/ready                  # readiness
```

## Testing

```bash
dotnet test
```

The tests boot the API in-memory with `WebApplicationFactory` and assert each endpoint returns the
right status code (and, for `not-found`, the localized message from the `.resx`).

## License

MIT — see [LICENSE](LICENSE).
