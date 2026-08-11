using JobAggregator.Api.Middleware;
using JobAggregator.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace JobAggregator.Infrastructure.Tests.Security;

public sealed class ProtectedPathAuthorizationMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldReturn401_WhenPathIsProtectedAndUserIsAnonymous()
    {
        var middleware = new ProtectedPathAuthorizationMiddleware(_ => Task.CompletedTask);
        var context = new DefaultHttpContext();
        context.Request.Path = "/admin/settings";
        context.RequestServices = BuildServices(new AllowAuthorizationService());

        await middleware.InvokeAsync(context, context.RequestServices.GetRequiredService<IAuthorizationService>());

        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn403_WhenPolicyFails()
    {
        var middleware = new ProtectedPathAuthorizationMiddleware(_ => Task.CompletedTask);
        var context = new DefaultHttpContext();
        context.Request.Path = "/providers/config";
        context.RequestServices = BuildServices(new DenyAuthorizationService());
        context.User = CreateAuthenticatedUser("user-1", roles: ["User"]);

        await middleware.InvokeAsync(context, context.RequestServices.GetRequiredService<IAuthorizationService>());

        Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ShouldBlockBrokenObjectAuthorizationForUsersPath()
    {
        var middleware = new ProtectedPathAuthorizationMiddleware(_ => Task.CompletedTask);
        var context = new DefaultHttpContext();
        context.Request.Path = "/users/user-2/preferences";
        context.RequestServices = BuildServices(new AllowAuthorizationService());
        context.User = CreateAuthenticatedUser("user-1", roles: ["User"]);

        await middleware.InvokeAsync(context, context.RequestServices.GetRequiredService<IAuthorizationService>());

        Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
    }

    private static ServiceProvider BuildServices(IAuthorizationService authorizationService)
    {
        return new ServiceCollection()
            .AddSingleton(authorizationService)
            .BuildServiceProvider();
    }

    private static ClaimsPrincipal CreateAuthenticatedUser(string userId, string[] roles)
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId) };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));
    }

    private sealed class AllowAuthorizationService : IAuthorizationService
    {
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
            => Task.FromResult(AuthorizationResult.Success());

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
            => Task.FromResult(AuthorizationResult.Success());
    }

    private sealed class DenyAuthorizationService : IAuthorizationService
    {
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
            => Task.FromResult(AuthorizationResult.Failed());

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
            => Task.FromResult(AuthorizationResult.Failed());
    }
}
