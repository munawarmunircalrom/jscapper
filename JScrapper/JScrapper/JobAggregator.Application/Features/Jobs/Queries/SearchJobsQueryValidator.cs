using FluentValidation;

namespace JobAggregator.Application.Features.Jobs.Queries;

public sealed class SearchJobsQueryValidator : AbstractValidator<SearchJobsQuery>
{
    public SearchJobsQueryValidator()
    {
        RuleFor(x => x.Keyword).MaximumLength(200);
        RuleFor(x => x.Title).MaximumLength(200);
        RuleFor(x => x.Company).MaximumLength(200);
        RuleFor(x => x.Location).MaximumLength(200);
        RuleFor(x => x.Experience).MaximumLength(100);
        RuleFor(x => x.EmploymentType).MaximumLength(100);
        RuleFor(x => x.Source).MaximumLength(100);

        RuleFor(x => x.MinSalary)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinSalary.HasValue);

        RuleFor(x => x.MaxSalary)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxSalary.HasValue);

        RuleFor(x => x)
            .Must(x => !x.MinSalary.HasValue || !x.MaxSalary.HasValue || x.MinSalary <= x.MaxSalary)
            .WithMessage("MinSalary must be less than or equal to MaxSalary.");

        RuleFor(x => x.Skills)
            .Must(skills => skills is null || skills.Count <= 20)
            .WithMessage("A maximum of 20 skills is allowed.");

        RuleForEach(x => x.Skills)
            .MaximumLength(64);

        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.SortBy)
            .Must(x => string.IsNullOrWhiteSpace(x) || x.Equals("postedDate", StringComparison.OrdinalIgnoreCase) || x.Equals("title", StringComparison.OrdinalIgnoreCase) || x.Equals("company", StringComparison.OrdinalIgnoreCase) || x.Equals("salary", StringComparison.OrdinalIgnoreCase) || x.Equals("experience", StringComparison.OrdinalIgnoreCase))
            .WithMessage("SortBy must be one of: postedDate, title, company, salary, experience.");
        RuleFor(x => x.SortDirection)
            .Must(x => string.IsNullOrWhiteSpace(x) || x.Equals("asc", StringComparison.OrdinalIgnoreCase) || x.Equals("desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage("SortDirection must be asc or desc.");
        RuleFor(x => x)
            .Must(x => !x.PostedFrom.HasValue || !x.PostedTo.HasValue || x.PostedFrom <= x.PostedTo)
            .WithMessage("PostedFrom must be less than or equal to PostedTo.");
    }
}
