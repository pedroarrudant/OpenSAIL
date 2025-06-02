using System.Text.Json;
using OpenSAIL.Cli.Models;

namespace OpenSAIL.Cli.Services;

public class ManifestGenerator
{
    public void Generate(SailManifest manifest, FileInfo output)
    {
        var json = JsonSerializer.Serialize(manifest, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        File.WriteAllText(output.FullName, json);
    }
}