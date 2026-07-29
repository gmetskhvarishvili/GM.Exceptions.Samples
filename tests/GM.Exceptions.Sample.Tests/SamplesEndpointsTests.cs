using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GM.Exceptions.Sample.Tests;

/// <summary>
/// Boots the API in-memory and verifies each endpoint's GM.Exceptions is mapped to the right HTTP
/// status, that the not-found message is localized from the .resx, and that validation errors flow
/// through. All cases live in one class so they run sequentially (the resource lookup caches once).
/// </summary>
public class SamplesEndpointsTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Theory]
    [InlineData("/samples/not-found", HttpStatusCode.NotFound)]
    [InlineData("/samples/already-exists", HttpStatusCode.Conflict)]
    [InlineData("/samples/bad-request", HttpStatusCode.BadRequest)]
    [InlineData("/samples/delete-restricted", HttpStatusCode.BadRequest)]
    [InlineData("/samples/validation", HttpStatusCode.BadRequest)]
    public async Task Endpoints_map_exceptions_to_status_codes(string url, HttpStatusCode expected)
    {
        var response = await _client.GetAsync(url);

        Assert.Equal(expected, response.StatusCode);
    }

    [Fact]
    public async Task NotFound_returns_the_localized_message_from_the_resx()
    {
        var response = await _client.GetAsync("/samples/not-found");

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("not found", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Validation_returns_the_per_field_errors()
    {
        var response = await _client.GetAsync("/samples/validation");

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("Name", body);
        Assert.Contains("Email", body);
    }
}
