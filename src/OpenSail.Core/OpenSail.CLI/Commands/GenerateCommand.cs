using System.CommandLine;
using OpenSAIL.Cli.Services;
using OpenSAIL.Cli.Models;

namespace OpenSAIL.Cli.Commands;

public static class GenerateCommand
{
    public static Command Create()
    {
        var command = new Command("generate", "Generate sail.manifest.json from your API project");

        var projectOption = new Option<FileInfo?>(
            name: "--project",
            description: "Optional path to the .csproj file (uses scanner if provided)");

        var outputOption = new Option<FileInfo>(
            name: "--output",
            description: "Output file for the manifest",
            getDefaultValue: () => new FileInfo("sail.manifest.json"));

        command.AddOption(projectOption);
        command.AddOption(outputOption);

        command.SetHandler((FileInfo? project, FileInfo output) =>
        {
            var manifest = new SailManifest();

            if (project is not null)
            {
                var scanner = new EndpointScanner();
                var endpoints = scanner.Scan(project.FullName);
                manifest.Endpoints = endpoints;
                Console.WriteLine($"✔️  Manifest generated from project: {project.Name}");
            }
            else
            {
                // fallback: mantém vazio ou faz outro processo, se desejar
                Console.WriteLine("⚠️  No project provided. Manifest will be empty.");
            }

            var generator = new ManifestGenerator();
            generator.Generate(manifest, output);
            Console.WriteLine($"📄 Manifest written to {output.FullName}");

        }, projectOption, outputOption);

        return command;
    }
}