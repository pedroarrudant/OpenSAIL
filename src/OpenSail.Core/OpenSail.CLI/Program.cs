using System.CommandLine;
using OpenSAIL.Cli.Commands;

var rootCommand = new RootCommand("OpenSAIL CLI - Semantic API Intent Layer");

rootCommand.AddCommand(GenerateCommand.Create());
rootCommand.AddCommand(ValidateCommand.Create());

return await rootCommand.InvokeAsync(args);