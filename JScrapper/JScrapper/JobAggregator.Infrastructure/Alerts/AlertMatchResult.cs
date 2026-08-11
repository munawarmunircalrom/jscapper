namespace JobAggregator.Infrastructure.Alerts;

internal sealed record AlertMatchResult(bool IsMatch, string Reason)
{
    public static AlertMatchResult Matched(string reason) => new(true, reason);
    public static AlertMatchResult NotMatched(string reason) => new(false, reason);
}
