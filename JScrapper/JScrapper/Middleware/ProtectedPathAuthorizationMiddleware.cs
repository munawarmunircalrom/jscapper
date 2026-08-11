using JobAggregator.Api.Security;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JobAggregator.Api.Middleware;

public sealed class ProtectedPathAuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IAuthorizationService authorizationService)
    {
        if (!ProtectedRoutes.TryResolvePolicy(context.Request.Path, out var policy, out var prefix))
        {
            await next(context);
            return;
        }

        if (context.User.Identity?.IsAuthenticated != true)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        if (prefix.Equals(new PathString(ProtectedRoutes.UsersPrefix)) && !CanAccessUserRoute(context))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        var authResult = await authorizationService.AuthorizeAsync(context.User, resource: null, policy);
        if (!authResult.Succeeded)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        await next(context);
    }

    private static bool CanAccessUserRoute(HttpContext context)
    {
        if (context.User.IsInRole("Admin"))
        {
            return true;
        }

        var path = context.Request.Path.Value;
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length < 2)
        {
            return true;
        }

        var routeUserId = segments[1];
        var claimUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? context.User.FindFirstValue("sub")
            ?? context.User.FindFirstValue("user_id");

        return !string.IsNullOrWhiteSpace(claimUserId)
            && string.Equals(routeUserId, claimUserId, StringComparison.OrdinalIgnoreCase);
    }
}
