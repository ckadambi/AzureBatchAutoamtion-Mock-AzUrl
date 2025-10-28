using System.Linq;
using System.Net;
using System.Text.Json;
using AzureJobAutomation.Models;
using AzureJobAutomation.Services;
using AzureJobAutomation.Utils;
using System.Threading.Tasks;
using System;
using Xunit;
using System.IO;
using System.Net.Http;

namespace AzureJobAutomation.Tests;


public class IntegrationTests
{
    static IntegrationTests()
    {
        RunIntegrationTestsAsync().GetAwaiter().GetResult();
    }

    private static async Task RunIntegrationTestsAsync()
    {
        var logger = new SimpleLogger();

        try
        {
            var cfgPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(cfgPath))
            {
                logger.Error($"Missing config: {cfgPath}");
                return;
            }

            var json = await File.ReadAllTextAsync(cfgPath);
            var jobConfig = JsonSerializer.Deserialize<JobConfig>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new JobConfig();
            EnvOverrides.Apply(jobConfig);

            if (jobConfig.Jobs.Count == 0)
            {
                logger.Warn("No jobs configured. Add at least one job via appsettings.json or environment variables.");
                return;
            }

            var httpClient = new HttpClient(new SocketsHttpHandler { AutomaticDecompression = DecompressionMethods.All })
            {
                Timeout = TimeSpan.FromSeconds(jobConfig.HttpTimeoutSeconds <= 0 ? 60 : jobConfig.HttpTimeoutSeconds)
            };

            var executor = new HttpJobExecutor(httpClient, logger)
            {
                MaxRetries = Math.Max(0, jobConfig.RetryCount),
                RetryDelay = TimeSpan.FromSeconds(Math.Clamp(jobConfig.RetryDelaySeconds, 0, 300))
            };

            logger.Info($"Starting {jobConfig.Jobs.Count} job(s)...");

            var tasks = jobConfig.Jobs.Select(job => executor.ExecuteJobAsync(job));
            var results = await Task.WhenAll(tasks);

            var success = results.Count(r => r.Success);
            var failed = results.Length - success;

            logger.Info("\n=== Summary ===");
            foreach (var r in results)
                logger.Info($"- {r.JobName}: {(r.Success ? "OK" : "FAILED")} ({r.StatusCode})");

            logger.Info($"Success: {success}, Failed: {failed}");
        }
        catch (Exception ex)
        {
            logger.Error($"Fatal error: {ex.Message}\n{ex}");
        }
    }

    private static JobConfig LoadConfig()
    {
        // var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "src", "AzureJobAutomation", "appsettings.json"));
        var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<JobConfig>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new JobConfig();
    }

    [Fact]
    public async Task Azure_Test_A_Positive_Should_Succeed()
    {
        var cfg = LoadConfig();
        var job = cfg.Jobs.First(j => j.Name.Contains("Azure Test A") && j.Name.Contains("Positive"));
        var exec = new HttpJobExecutor(new HttpClient(), new SimpleLogger());
        var result = await exec.ExecuteJobAsync(job);
        Assert.True(result.Success);
    }

    [Fact(Skip = "Azure Test A Negative - remove Skip to run live")]
    public async Task Azure_Test_A_Negative_Should_Fail()
    {
        var cfg = LoadConfig();
        var job = cfg.Jobs.First(j => j.Name.Contains("Azure Test A") && j.Name.Contains("Negative"));
        var exec = new HttpJobExecutor(new HttpClient(), new SimpleLogger());
        var result = await exec.ExecuteJobAsync(job);
        Assert.False(result.Success);
    }

    [Fact(Skip = "Azure Test B Positive - remove Skip to run live")]
    public async Task Azure_Test_B_Positive_Should_Succeed()
    {
        var cfg = LoadConfig();
        var job = cfg.Jobs.First(j => j.Name.Contains("Azure Test B") && j.Name.Contains("Positive"));
        var exec = new HttpJobExecutor(new HttpClient(), new SimpleLogger());
        var result = await exec.ExecuteJobAsync(job);
        Assert.True(result.Success);
    }

    [Fact(Skip = "Azure Test B Negative - remove Skip to run live")]
    public async Task Azure_Test_B_Negative_Should_Fail()
    {
        var cfg = LoadConfig();
        var job = cfg.Jobs.First(j => j.Name.Contains("Azure Test B") && j.Name.Contains("Negative"));
        var exec = new HttpJobExecutor(new HttpClient(), new SimpleLogger());
        var result = await exec.ExecuteJobAsync(job);
        Assert.False(result.Success);
    }
}
