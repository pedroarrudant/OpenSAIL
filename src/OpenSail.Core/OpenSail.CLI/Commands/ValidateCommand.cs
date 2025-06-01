using System.CommandLine;
using OpenSAIL.Cli.Services;

namespace OpenSAIL.Cli.Commands;

public static class ValidateCommand
{
    public static Command Create()
    {
        var command = new Command("validate", "Validate an existing sail.manifest.json file");

        var fileOption = new Option<FileInfo>(
                name: "--file",
                description: "Path to the manifest file to validate")
            { IsRequired = true };

        command.AddOption(fileOption);

        command.SetHandler((FileInfo file) =>
        {
            var validator = new ManifestValidator();
            var result = validator.Validate(file);

            if (!result.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Manifest is invalid:");
                foreach (var error in result.Errors)
                    Console.WriteLine($"❌ {error}");
                Console.ResetColor();
                Environment.Exit(1);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✅ Manifest is valid!");
                Console.ResetColor();
            }

        }, fileOption);

        return command;
    }
}