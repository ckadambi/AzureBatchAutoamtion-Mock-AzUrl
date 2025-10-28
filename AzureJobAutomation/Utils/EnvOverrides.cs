using System.Text.Json;
using System.Text.Json.Nodes;
using AzureJobAutomation.Models;

namespace AzureJobAutomation.Utils;

public static class EnvOverrides
{
    public static void Apply(JobConfig cfg)
    {
        if (int.TryParse(Environment.GetEnvironmentVariable("HTTP_TIMEOUT_SECONDS"), out var t)) cfg.HttpTimeoutSeconds = t;
        if (int.TryParse(Environment.GetEnvironmentVariable("RETRY_COUNT"), out var r)) cfg.RetryCount = r;
        if (int.TryParse(Environment.GetEnvironmentVariable("RETRY_DELAY_SECONDS"), out var d)) cfg.RetryDelaySeconds = d;

        var urlsCsv = Environment.GetEnvironmentVariable("JOB_URLS");
        if (!string.IsNullOrWhiteSpace(urlsCsv))
        {
            var urls = urlsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
            cfg.Jobs.Clear();
            foreach (var url in urls)
                cfg.Jobs.Add(new JobDefinition { Name = url, Url = url });
        }
    }
}
