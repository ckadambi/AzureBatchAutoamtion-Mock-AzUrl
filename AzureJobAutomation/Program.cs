// using System.Net;
// using System.Text.Json;
// using AzureJobAutomation.Models;
// using AzureJobAutomation.Services;
// using AzureJobAutomation.Utils;

// var logger = new SimpleLogger();

// try
// {
//     var cfgPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
//     if (!File.Exists(cfgPath))
//     {
//         logger.Error($"Missing config: {cfgPath}");
//         return;
//     }

//     var json = await File.ReadAllTextAsync(cfgPath);
//     var jobConfig = JsonSerializer.Deserialize<JobConfig>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new JobConfig();
//     EnvOverrides.Apply(jobConfig);

//     if (jobConfig.Jobs.Count == 0)
//     {
//         logger.Warn("No jobs configured. Add at least one job via appsettings.json or environment variables.");
//         return;
//     }

//     var httpClient = new HttpClient(new SocketsHttpHandler { AutomaticDecompression = DecompressionMethods.All })
//     {
//         Timeout = TimeSpan.FromSeconds(jobConfig.HttpTimeoutSeconds <= 0 ? 60 : jobConfig.HttpTimeoutSeconds)
//     };

//     var executor = new HttpJobExecutor(httpClient, logger)
//     {
//         MaxRetries = Math.Max(0, jobConfig.RetryCount),
//         RetryDelay = TimeSpan.FromSeconds(Math.Clamp(jobConfig.RetryDelaySeconds, 0, 300))
//     };

//     logger.Info($"Starting {jobConfig.Jobs.Count} job(s)...");

//     var tasks = jobConfig.Jobs.Select(job => executor.ExecuteJobAsync(job));
//     var results = await Task.WhenAll(tasks);

//     var success = results.Count(r => r.Success);
//     var failed = results.Length - success;

//     logger.Info("\n=== Summary ===");
//     foreach (var r in results)
//         logger.Info($"- {r.JobName}: {(r.Success ? "OK" : "FAILED")} ({r.StatusCode})");

//     logger.Info($"Success: {success}, Failed: {failed}");
// }
// catch (Exception ex)
// {
//     logger.Error($"Fatal error: {ex.Message}\n{ex}");
// }
