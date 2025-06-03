using System.IO;
using OpenSAIL.Cli.Services;
using Xunit;

namespace OpenSail.Tests.Services;

public class ManifestValidatorTests
{
    [Fact]
    public void Validate_MissingFile_ReturnsError()
    {
        var validator = new ManifestValidator();
        var result = validator.Validate(new FileInfo(Path.Combine(Path.GetTempPath(), "nonexistent.json")));
        Assert.False(result.IsValid);
        Assert.Contains("File does not exist.", result.Errors);
    }

    [Fact]
    public void Validate_InvalidJson_ReturnsError()
    {
        var file = Path.GetTempFileName();
        File.WriteAllText(file, "{ invalid }");
        var validator = new ManifestValidator();

        var result = validator.Validate(new FileInfo(file));

        Assert.False(result.IsValid);
        Assert.Contains("Invalid JSON", result.Errors[0]);
    }

    [Fact]
    public void Validate_MissingRequiredFields_ReturnsError()
    {
        var file = Path.GetTempFileName();
        File.WriteAllText(file, "{}");
        var validator = new ManifestValidator();

        var result = validator.Validate(new FileInfo(file));

        Assert.False(result.IsValid);
        Assert.Contains("No endpoints defined.", result.Errors);
    }
}
