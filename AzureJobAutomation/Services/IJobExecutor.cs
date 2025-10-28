using AzureJobAutomation.Models;

namespace AzureJobAutomation.Services;

public interface IJobExecutor
{
    Task<JobResult> ExecuteJobAsync(JobDefinition job, CancellationToken ct = default);
}
