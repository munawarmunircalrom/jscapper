namespace JobAggregator.Api.Middleware;

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        return app.UseMiddleware<SecurityHeadersMiddleware>();
    }

    public static IApplicationBuilder UseRequestHardening(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestHardeningMiddleware>();
    }

    public static IApplicationBuilder UseProtectedPathAuthorization(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ProtectedPathAuthorizationMiddleware>();
    }

    public static IApplicationBuilder UseAuditLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AuditLoggingMiddleware>();
    }
}
