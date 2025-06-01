using System.Text.Json.Serialization;

namespace OpenSAIL.Cli.Models;
public class SailManifest
{
    [JsonPropertyName("endpoints")]
    public List<SailEndpoint> Endpoints { get; set; } = new();
}

public class SailEndpoint
{
    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("intent")]
    public string Intent { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("meaning")]
    public string Meaning { get; set; }

    [JsonPropertyName("examples")]
    public string[] Examples { get; set; }
}