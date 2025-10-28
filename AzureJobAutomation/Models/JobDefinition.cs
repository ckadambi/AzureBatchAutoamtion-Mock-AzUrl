using System.Text.Json.Nodes;

namespace AzureJobAutomation.Models;

public sealed class JobDefinition
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = "POST";
    public Dictionary<string, string>? Headers { get; set; }
    public JsonObject? Body { get; set; }
}
