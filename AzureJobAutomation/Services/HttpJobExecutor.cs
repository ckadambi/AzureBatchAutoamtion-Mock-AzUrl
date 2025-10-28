using System.Net;
using System.Text;
using AzureJobAutomation.Models;
using AzureJobAutomation.Utils;

namespace AzureJobAutomation.Services;

public sealed class HttpJobExecutor(HttpClient http, SimpleLogger logger) : IJobExecutor
{
    public int MaxRetries { get; set; } = 2;
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(2);

    public async Task<JobResult> ExecuteJobAsync(JobDefinition job, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(job.Url))
            throw new ArgumentException("Job Url is required", nameof(job));

        var attempts = 0;
        HttpResponseMessage? resp = null;
        string? body = null;

        while (true)
        {
            attempts++;
            try
            {
                using var req = new HttpRequestMessage(new HttpMethod(job.Method ?? "POST"), job.Url);
                if (job.Headers != null)
                {
                    foreach (var kv in job.Headers)
                    {
                        if (!req.Headers.TryAddWithoutValidation(kv.Key, kv.Value))
                        {
                            req.Content ??= new StringContent(string.Empty);
                            req.Content.Headers.TryAddWithoutValidation(kv.Key, kv.Value);
                        }
                    }
                }
                if (job.Body != null)
                    req.Content = new StringContent(job.Body.ToJsonString(), Encoding.UTF8, "application/json");

                resp = await http.SendAsync(req, ct);
                body = await resp.Content.ReadAsStringAsync(ct);

                if ((int)resp.StatusCode is >= 200 and < 300)
                {
                    return new JobResult { JobName = job.Name, Success = true, StatusCode = resp.StatusCode, ResponseBody = body, Attempts = attempts };
                }
            }
            catch (Exception ex) when (ex is TaskCanceledException or HttpRequestException)
            {
                logger.Warn($"[{job.Name}] Attempt {attempts} failed: {ex.Message}");
            }

            if (attempts > MaxRetries)
                return new JobResult { JobName = job.Name, Success = false, StatusCode = resp?.StatusCode, ResponseBody = body, Attempts = attempts };

            await Task.Delay(RetryDelay, ct);
        }
    }
}
