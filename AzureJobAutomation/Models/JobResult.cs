using System.Net;

namespace AzureJobAutomation.Models;

public sealed class JobResult
{
    public required string JobName { get; init; }
    public bool Success { get; init; }
    public HttpStatusCode? StatusCode { get; init; }
    public string? ResponseBody { get; init; }
    public int Attempts { get; init; }
}
