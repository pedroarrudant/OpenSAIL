using System.Text.Json;
using OpenSAIL.Cli.Models;

namespace OpenSAIL.Cli.Services;

public class ManifestValidator
{
    public (bool IsValid, List<string> Errors) Validate(FileInfo file)
    {
        var errors = new List<string>();

        if (!file.Exists)
        {
            errors.Add("File does not exist.");
            return (false, errors);
        }

        SailManifest? manifest = null;

        try
        {
            var content = File.ReadAllText(file.FullName);
            manifest = JsonSerializer.Deserialize<SailManifest>(content);
        }
        catch (Exception ex)
        {
            errors.Add($"Invalid JSON: {ex.Message}");
            return (false, errors);
        }

        if (manifest?.Endpoints == null || !manifest.Endpoints.Any())
            errors.Add("No endpoints defined.");

        foreach (var ep in manifest?.Endpoints ?? Enumerable.Empty<SailEndpoint>())
        {
            if (string.IsNullOrWhiteSpace(ep.Path))
                errors.Add("An endpoint is missing 'path'.");

            if (string.IsNullOrWhiteSpace(ep.Method))
                errors.Add($"Endpoint {ep.Path} is missing 'method'.");

            if (string.IsNullOrWhiteSpace(ep.Intent))
                errors.Add($"Endpoint {ep.Path} is missing 'intent'.");

            if (string.IsNullOrWhiteSpace(ep.Description))
                errors.Add($"Endpoint {ep.Path} is missing 'description'.");
        }

        return (errors.Count == 0, errors);
    }
}