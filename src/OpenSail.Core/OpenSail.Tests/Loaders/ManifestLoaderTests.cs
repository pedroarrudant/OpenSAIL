using System.IO;
using OpenSAIL.Core.Loaders;
using Xunit;

namespace OpenSail.Tests.Loaders;

public class ManifestLoaderTests
{
    [Fact]
    public void Load_ValidManifest_ReturnsManifest()
    {
        var json = "{" +
                    "\"endpoints\": [{" +
                    "\"path\": \"/test\"," +
                    "\"method\": \"get\"," +
                    "\"intent\": \"Test\"," +
                    "\"description\": \"desc\"," +
                    "\"meaning\": \"mean\"" +
                    "}]" +
                    "}";
        var file = Path.GetTempFileName();
        File.WriteAllText(file, json);

        var manifest = SailManifestLoader.Load(file);

        Assert.NotNull(manifest);
        Assert.NotNull(manifest.Endpoints);
        Assert.Single(manifest.Endpoints);
    }
}
