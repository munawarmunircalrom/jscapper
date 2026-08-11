using JobAggregator.Api.Security;
using JobAggregator.Domain.Entities;
using JobAggregator.Infrastructure.Persistence;
using System.Security.Claims;

namespace JobAggregator.Api.Middleware;

public sealed class AuditLoggingMiddleware(RequestDelegate next, ILogger<AuditLoggingMiddleware> logger)
{
    private static readonly HashSet<string> AuditedMethods = new(StringComparer.OrdinalIgnoreCase)
    {
        HttpMethods.Post,
        HttpMethods.Put,
        HttpMethods.Patch,
        HttpMethods.Delete
    };

    public async Task InvokeAsync(HttpContext context, JobAggregatorDbContext dbContext)
    {
        var shouldAudit = ShouldAudit(context.Request);
        var startedAt = DateTimeOffset.UtcNow;

        await next(context);

        if (!shouldAudit)
        {
            return;
        }

        try
        {
            var userId = TryParseUserId(context.User);
            var entityName = ResolveEntityName(context.Request.Path);
            var entityId = ResolveEntityId(context.Request.Path);
            var statusCode = context.Response.StatusCode;

            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Action = TrimToLength($"{context.Request.Method} {context.Request.Path}", 120),
                EntityName = TrimToLength(entityName, 120),
                EntityId = TrimToLength(entityId, 120),
                ChangesJson = TrimToLength($"{{\"statusCode\":{statusCode},\"durationMs\":{(DateTimeOffset.UtcNow - startedAt).TotalMilliseconds:0},\"query\":\"{context.Request.QueryString.Value}\"}}", 4000),
                IpAddress = TrimToLength(context.Connection.RemoteIpAddress?.ToString(), 64),
                UserAgent = TrimToLength(context.Request.Headers.UserAgent.ToString(), 400)
            };

            dbContext.AuditLogs.Add(auditLog);
            await dbContext.SaveChangesAsync(context.RequestAborted);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to persist audit log entry for request path {Path}.", context.Request.Path);
        }
    }

    private static bool ShouldAudit(HttpRequest request)
    {
        if (!AuditedMethods.Contains(request.Method))
        {
            return false;
        }

        return ProtectedRoutes.TryResolvePolicy(request.Path, out _, out _);
    }

    private static string ResolveEntityName(PathString path)
    {
        var value = path.Value ?? string.Empty;
        var segments = value.Split('/', StringSplitOptions.RemoveEmptyEntries);
        return segments.Length == 0 ? "unknown" : segments[0];
    }

    private static string ResolveEntityId(PathString path)
    {
        var value = path.Value ?? string.Empty;
        var segments = value.Split('/', StringSplitOptions.RemoveEmptyEntries);
        return segments.Length < 2 ? "n/a" : segments[1];
    }

    private static Guid? TryParseUserId(ClaimsPrincipal user)
    {
        var idClaim = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub")
            ?? user.FindFirstValue("user_id");

        return Guid.TryParse(idClaim, out var parsed) ? parsed : null;
    }

    private static string TrimToLength(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return value.Length <= maxLength ? value : value[..maxLength];
    }
}
