using FluentValidation;
using JobAggregator.Application.DTOs;

namespace JobAggregator.Application.Features.Alerts;

public sealed class UpsertJobAlertRequestValidator : AbstractValidator<UpsertJobAlertRequest>
{
    public UpsertJobAlertRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Keywords).MaximumLength(500);
        RuleFor(x => x.Location).MaximumLength(200);
        RuleFor(x => x.Experience).MaximumLength(100);
        RuleFor(x => x.EmploymentType).MaximumLength(100);

        RuleFor(x => x.MinSalary)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinSalary.HasValue);

        RuleFor(x => x.MaxSalary)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxSalary.HasValue);

        RuleFor(x => x)
            .Must(x => !x.MinSalary.HasValue || !x.MaxSalary.HasValue || x.MinSalary <= x.MaxSalary)
            .WithMessage("MinSalary must be less than or equal to MaxSalary.");

        RuleFor(x => x.FrequencyMinutes).InclusiveBetween(5, 1440);

        RuleFor(x => x.Skills)
            .Must(skills => skills is null || skills.Count <= 25)
            .WithMessage("A maximum of 25 skills is allowed.");

        RuleForEach(x => x.Skills).MaximumLength(64);

        RuleFor(x => x.Sources)
            .Must(sources => sources is null || sources.Count <= 10)
            .WithMessage("A maximum of 10 sources is allowed.");

        RuleForEach(x => x.Sources).MaximumLength(64);
    }
}
