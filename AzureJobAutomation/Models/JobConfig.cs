namespace AzureJobAutomation.Models;

public sealed class JobConfig
{
    public int HttpTimeoutSeconds { get; set; } = 60;
    public int RetryCount { get; set; } = 2;
    public int RetryDelaySeconds { get; set; } = 2;
    public List<JobDefinition> Jobs { get; set; } = new();
}
