using JobAggregator.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace JobAggregator.Infrastructure.Tests.Security;

public sealed class RequestHardeningMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldReturnBadRequest_WhenPathTraversalPatternDetected()
    {
        var middleware = new RequestHardeningMiddleware(_ => Task.CompletedTask, NullLogger<RequestHardeningMiddleware>.Instance);
        var context = new DefaultHttpContext();
        context.Request.Path = "/users/../secrets";

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturnBadRequest_WhenPotentialSsrfInputDetected()
    {
        var middleware = new RequestHardeningMiddleware(_ => Task.CompletedTask, NullLogger<RequestHardeningMiddleware>.Instance);
        var context = new DefaultHttpContext();
        context.Request.Path = "/providers/update";
        context.Request.QueryString = new QueryString("?sourceUrl=http://127.0.0.1/internal");

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
    }
}
