using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TopEndLibraryHub.Middleware;

namespace TopEndLibraryHub.Tests;

public class ApiKeyTests
{
    [Theory]
    [InlineData(null, 401, false)]
    [InlineData("wrong", 401, false)]
    [InlineData("test-only-key", 200, true)]
    public async Task Middleware_RequiresTheConfiguredApiKey(string? key, int status, bool expectedCalled)
    {
        var called = false;
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        { ["CatalogueApi:ApiKey"] = "test-only-key" }).Build();
        var middleware = new CatalogueApiKeyMiddleware(_ => { called = true; return Task.CompletedTask; }, config);
        var context = new DefaultHttpContext(); context.Request.Path = "/api/catalogue/available";
        context.Response.Body = new MemoryStream();
        if (key is not null) context.Request.Headers["X-API-Key"] = key;
        await middleware.InvokeAsync(context);
        Assert.Equal(status, context.Response.StatusCode); Assert.Equal(expectedCalled, called);
    }

    [Fact]
    public async Task UnconfiguredApi_FailsClosed()
    {
        var config = new ConfigurationBuilder().Build();
        var middleware = new CatalogueApiKeyMiddleware(_ => throw new Exception("Should not continue"), config);
        var context = new DefaultHttpContext(); context.Request.Path = "/api/catalogue";
        context.Response.Body = new MemoryStream();
        await middleware.InvokeAsync(context); Assert.Equal(503, context.Response.StatusCode);
    }
}
