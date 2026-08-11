namespace JobAggregator.Api.Security;

public static class ProtectedRoutes
{
    public const string AdminPrefix = "/admin";
    public const string ProvidersPrefix = "/providers";
    public const string IngestionPrefix = "/ingestion";
    public const string UsersPrefix = "/users";
    public const string AlertsPrefix = "/alerts";

    public static readonly IReadOnlyList<(PathString Prefix, string Policy)> RoutePolicies =
    [
        (new PathString(AdminPrefix), AuthorizationPolicies.Admin),
        (new PathString(ProvidersPrefix), AuthorizationPolicies.ProviderManagement),
        (new PathString(IngestionPrefix), AuthorizationPolicies.IngestionManagement),
        (new PathString(UsersPrefix), AuthorizationPolicies.UserDataAccess),
        (new PathString(AlertsPrefix), AuthorizationPolicies.AlertsAccess)
    ];

    public static bool TryResolvePolicy(PathString path, out string policy, out PathString prefix)
    {
        foreach (var routePolicy in RoutePolicies)
        {
            if (path.StartsWithSegments(routePolicy.Prefix, StringComparison.OrdinalIgnoreCase))
            {
                policy = routePolicy.Policy;
                prefix = routePolicy.Prefix;
                return true;
            }
        }

        policy = string.Empty;
        prefix = PathString.Empty;
        return false;
    }
}
