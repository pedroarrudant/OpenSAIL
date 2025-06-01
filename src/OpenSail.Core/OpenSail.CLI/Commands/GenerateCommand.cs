using System.CommandLine;
using OpenSAIL.Cli.Services;

namespace OpenSAIL.Cli.Commands;

public static class GenerateCommand
{
    public static Command Create()
    {
        var command = new Command("generate", "Generate sail.manifest.json from your API project");

        var projectOption = new Option<FileInfo>(
                name: "--project",
                description: "Path to the .csproj file")
            { IsRequired = true };

        var outputOption = new Option<FileInfo>(
            name: "--output",
            description: "Output file for the manifest",
            getDefaultValue: () => new FileInfo("sail.manifest.json"));

        command.AddOption(projectOption);
        command.AddOption(outputOption);

        command.SetHandler((FileInfo project, FileInfo output) =>
        {
            var generator = new ManifestGenerator();
            generator.Generate(project, output);
        }, projectOption, outputOption);

        return command;
    }
}