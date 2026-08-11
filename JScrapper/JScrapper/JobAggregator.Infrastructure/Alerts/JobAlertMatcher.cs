using JobAggregator.Domain.Entities;

namespace JobAggregator.Infrastructure.Alerts;

internal static class JobAlertMatcher
{
    public static AlertMatchResult Match(Job job, IReadOnlyCollection<string> sources, JobAlert alert)
    {
        if (!alert.IsEnabled)
        {
            return AlertMatchResult.NotMatched("disabled");
        }

        if (!ContainsText(job, alert.Keywords))
        {
            return AlertMatchResult.NotMatched("keyword");
        }

        if (!ContainsLocation(job, alert.Location))
        {
            return AlertMatchResult.NotMatched("location");
        }

        if (!MatchesSkills(job, alert.SkillsCsv))
        {
            return AlertMatchResult.NotMatched("skills");
        }

        if (!MatchesSalary(job, alert.MinSalary, alert.MaxSalary))
        {
            return AlertMatchResult.NotMatched("salary");
        }

        if (!ContainsField(job.Seniority, alert.Experience))
        {
            return AlertMatchResult.NotMatched("experience");
        }

        if (!ContainsField(job.EmploymentType, alert.EmploymentType))
        {
            return AlertMatchResult.NotMatched("employmentType");
        }

        if (!MatchesRemote(job, alert.Remote))
        {
            return AlertMatchResult.NotMatched("remote");
        }

        if (!MatchesSources(sources, alert.SourcesCsv))
        {
            return AlertMatchResult.NotMatched("sources");
        }

        return AlertMatchResult.Matched("all");
    }

    private static bool ContainsText(Job job, string? keywords)
    {
        if (string.IsNullOrWhiteSpace(keywords))
        {
            return true;
        }

        var text = string.Join(' ', [job.Title, job.Description, job.Company?.Name, job.SearchText]).ToLowerInvariant();
        var tokens = SplitCsvOrSpace(keywords);

        return tokens.Any(token => text.Contains(token, StringComparison.OrdinalIgnoreCase));
    }

    private static bool ContainsLocation(Job job, string? location)
    {
        if (string.IsNullOrWhiteSpace(location))
        {
            return true;
        }

        var normalized = location.Trim();
        var raw = job.JobLocation?.RawText ?? string.Empty;
        var city = job.JobLocation?.City ?? string.Empty;
        var country = job.JobLocation?.Country ?? string.Empty;

        return raw.Contains(normalized, StringComparison.OrdinalIgnoreCase)
            || city.Contains(normalized, StringComparison.OrdinalIgnoreCase)
            || country.Contains(normalized, StringComparison.OrdinalIgnoreCase);
    }

    private static bool MatchesSkills(Job job, string? skillsCsv)
    {
        var requiredSkills = SplitCsvOrSpace(skillsCsv);
        if (requiredSkills.Count == 0)
        {
            return true;
        }

        var jobSkills = job.JobSkills
            .Select(s => s.Name.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return requiredSkills.Any(skill => jobSkills.Contains(skill));
    }

    private static bool MatchesSalary(Job job, decimal? minSalary, decimal? maxSalary)
    {
        if (!minSalary.HasValue && !maxSalary.HasValue)
        {
            return true;
        }

        var salary = job.JobSalary;
        if (salary is null)
        {
            return false;
        }

        var min = salary.MinAmount;
        var max = salary.MaxAmount;

        if (minSalary.HasValue)
        {
            var threshold = minSalary.Value;
            var hasMinMatch = (max.HasValue && max.Value >= threshold) || (min.HasValue && min.Value >= threshold);
            if (!hasMinMatch)
            {
                return false;
            }
        }

        if (maxSalary.HasValue)
        {
            var threshold = maxSalary.Value;
            var hasMaxMatch = (min.HasValue && min.Value <= threshold) || (max.HasValue && max.Value <= threshold);
            if (!hasMaxMatch)
            {
                return false;
            }
        }

        return true;
    }

    private static bool ContainsField(string? fieldValue, string? filterValue)
    {
        if (string.IsNullOrWhiteSpace(filterValue))
        {
            return true;
        }

        return !string.IsNullOrWhiteSpace(fieldValue)
            && fieldValue.Contains(filterValue.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    private static bool MatchesRemote(Job job, bool? remote)
    {
        if (!remote.HasValue)
        {
            return true;
        }

        return remote.Value
            ? string.Equals(job.WorkMode, "Remote", StringComparison.OrdinalIgnoreCase)
            : !string.Equals(job.WorkMode, "Remote", StringComparison.OrdinalIgnoreCase);
    }

    private static bool MatchesSources(IReadOnlyCollection<string> sources, string? sourcesCsv)
    {
        var requiredSources = SplitCsvOrSpace(sourcesCsv);
        if (requiredSources.Count == 0)
        {
            return true;
        }

        return sources.Any(source => requiredSources.Contains(source));
    }

    private static HashSet<string> SplitCsvOrSpace(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return [];
        }

        return input
            .Split([',', ';'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .SelectMany(part => part.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            .Select(x => x.Trim())
            .Where(x => x.Length > 0)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }
}
