namespace JobAggregator.Api.Security;

public static class AuthorizationPolicies
{
    public const string Admin = "AdminPolicy";
    public const string ProviderManagement = "ProviderManagementPolicy";
    public const string IngestionManagement = "IngestionManagementPolicy";
    public const string UserDataAccess = "UserDataAccessPolicy";
    public const string AlertsAccess = "AlertsAccessPolicy";
}
