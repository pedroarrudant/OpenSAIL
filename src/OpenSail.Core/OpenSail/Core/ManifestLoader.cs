using System.IO;
using System.Text.Json;
using OpenSail.Core.Models;

namespace OpenSAIL.Core.Loaders
{
    public static class SailManifestLoader
    {
        public static SailManifest Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Manifest file not found at: {path}");

            var json = File.ReadAllText(path);
            var manifest = JsonSerializer.Deserialize<SailManifest>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return manifest ?? new SailManifest();
        }
    }
}