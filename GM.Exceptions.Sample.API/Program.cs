using System.Resources;
using GM.Exceptions.Sample.API.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

// The .resx resources ship with English as their neutral/fallback culture.
[assembly: NeutralResourcesLanguage("en")]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Return RFC 9457 problem details, and map GM.Exceptions to HTTP responses.
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseExceptionHandler();

// Liveness must not depend on downstream dependencies; readiness may add checks later.
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("/health/ready");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

await app.RunAsync();

// Exposed so the integration test project can bootstrap the app via WebApplicationFactory.
public partial class Program
{
    // Only used as a WebApplicationFactory<Program> marker; never instantiated directly.
    protected Program() { }
}
