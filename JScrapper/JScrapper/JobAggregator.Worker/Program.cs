using JobAggregator.Application;
using JobAggregator.Application.Configuration;
using JobAggregator.Infrastructure;
using JobAggregator.Worker.Jobs;
using JobAggregator.Worker.Scheduling;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.Configure<SearchPlatformOptions>(builder.Configuration.GetSection("SearchPlatforms"));

builder.Services.Configure<IngestionSchedulingOptions>(builder.Configuration.GetSection("Ingestion"));

var schedules = builder.Configuration
    .GetSection("Ingestion:Providers")
    .Get<List<ProviderIngestionScheduleOptions>>()
    ?? [];

builder.Services.AddQuartz(quartz =>
{
    quartz.UseMicrosoftDependencyInjectionJobFactory();

    foreach (var schedule in schedules.Where(x => x.Enabled && !string.IsNullOrWhiteSpace(x.ProviderName)))
    {
        var providerName = schedule.ProviderName.Trim();
        var jobKey = new JobKey($"ingestion-{providerName}", "providers");

        quartz.AddJob<ProviderIngestionJob>(options => options
            .WithIdentity(jobKey)
            .StoreDurably()
            .UsingJobData(ProviderIngestionJob.ProviderNameKey, providerName)
            .UsingJobData(ProviderIngestionJob.TimeoutSecondsKey, schedule.TimeoutSeconds)
            .UsingJobData(ProviderIngestionJob.MaxAttemptsKey, schedule.MaxAttempts)
            .UsingJobData(ProviderIngestionJob.RetryBaseDelaySecondsKey, schedule.RetryBaseDelaySeconds));

        quartz.AddTrigger(options => options
            .ForJob(jobKey)
            .WithIdentity($"trigger-{providerName}", "providers")
            .WithCronSchedule(schedule.Cron, cron => cron.WithMisfireHandlingInstructionDoNothing()));
    }
});

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

var host = builder.Build();
host.Run();
