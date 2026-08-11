namespace JobAggregator.Contracts.Common;

public sealed class ApiErrorResponse
{
    public required string Code { get; init; }
    public required string Message { get; init; }
    public IReadOnlyCollection<string>? Details { get; init; }
    public string? TraceId { get; init; }
}
