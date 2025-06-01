using System.Text.Json;
using OpenSAIL.Cli.Models;

namespace OpenSAIL.Cli.Services;

public class ManifestGenerator
{
    public void Generate(FileInfo project, FileInfo output)
    {
        // TODO: parse project or extract OpenAPI (basic stub)
        var manifest = new SailManifest
        {
            Endpoints = new List<SailEndpoint>
            {
                new()
                {
                    Path = "/example",
                    Method = "get",
                    Intent = "get_example",
                    Description = "An example endpoint.",
                    Meaning = "Returns sample data.",
                    Examples = new[] { "Give me an example", "Sample call" }
                }
            }
        };

        var json = JsonSerializer.Serialize(manifest, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(output.FullName, json);
        Console.WriteLine($"Manifest generated at: {output.FullName}");
    }
}