using JobAggregator.Api.Security;
using JobAggregator.Contracts.Common;
using System.Net;

namespace JobAggregator.Api.Middleware;

public sealed class RequestHardeningMiddleware(RequestDelegate next, ILogger<RequestHardeningMiddleware> logger)
{
    private static readonly string[] RedirectQueryKeys = ["returnUrl", "redirectUrl", "nextUrl", "continue"];
    private static readonly string[] UrlInputKeys = ["sourceUrl", "providerUrl", "callbackUrl", "webhookUrl", "endpointUrl", "targetUrl"];

    public async Task InvokeAsync(HttpContext context)
    {
        if (HasPathTraversalPattern(context.Request.Path))
        {
            logger.LogWarning("Path traversal pattern blocked for request path {Path}.", context.Request.Path);
            await WriteBlockedResponseAsync(context, "path_traversal_blocked", "The requested path is invalid.");
            return;
        }

        if (ProtectedRoutes.TryResolvePolicy(context.Request.Path, out _, out _))
        {
            if (HasUnsafeRedirectInput(context.Request.Query))
            {
                logger.LogWarning("Open redirect pattern blocked for request path {Path}.", context.Request.Path);
                await WriteBlockedResponseAsync(context, "open_redirect_blocked", "Unsafe redirect target is not allowed.");
                return;
            }

            if (HasPotentialSsrfInput(context.Request.Query))
            {
                logger.LogWarning("Potential SSRF target blocked for request path {Path}.", context.Request.Path);
                await WriteBlockedResponseAsync(context, "ssrf_blocked", "The provided URL target is not allowed.");
                return;
            }
        }

        await next(context);
    }

    private static bool HasPathTraversalPattern(PathString path)
    {
        var value = path.Value ?? string.Empty;
        return value.Contains("..", StringComparison.Ordinal) || value.Contains("%2e%2e", StringComparison.OrdinalIgnoreCase);
    }

    private static bool HasUnsafeRedirectInput(IQueryCollection query)
    {
        foreach (var key in RedirectQueryKeys)
        {
            if (!query.TryGetValue(key, out var values))
            {
                continue;
            }

            foreach (var value in values)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                if (Uri.TryCreate(value, UriKind.Absolute, out var absoluteUri))
                {
                    return !absoluteUri.IsLoopback && !string.IsNullOrWhiteSpace(absoluteUri.Host);
                }
            }
        }

        return false;
    }

    private static bool HasPotentialSsrfInput(IQueryCollection query)
    {
        foreach (var key in UrlInputKeys)
        {
            if (!query.TryGetValue(key, out var values))
            {
                continue;
            }

            foreach (var value in values)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
                {
                    continue;
                }

                if (uri.IsLoopback)
                {
                    return true;
                }

                if (IPAddress.TryParse(uri.Host, out var ipAddress) && IsPrivateAddress(ipAddress))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool IsPrivateAddress(IPAddress address)
    {
        var bytes = address.GetAddressBytes();

        if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            return bytes[0] switch
            {
                10 => true,
                127 => true,
                172 when bytes[1] >= 16 && bytes[1] <= 31 => true,
                192 when bytes[1] == 168 => true,
                _ => false
            };
        }

        return IPAddress.IsLoopback(address) || address.IsIPv6LinkLocal || address.IsIPv6SiteLocal;
    }

    private static async Task WriteBlockedResponseAsync(HttpContext context, string code, string message)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new ApiErrorResponse
        {
            Code = code,
            Message = message,
            TraceId = context.TraceIdentifier
        });
    }
}
