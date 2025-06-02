using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OpenSAIL.Cli.Models;

namespace OpenSAIL.Cli.Services
{
    public class EndpointScanner
    {
        public List<SailEndpoint> Scan(string projectPath)
        {
            var endpoints = new List<SailEndpoint>();

            var dir = Path.GetDirectoryName(projectPath);
            if (dir == null) return endpoints;

            var csFiles = Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories);

            foreach (var file in csFiles)
            {
                var code = File.ReadAllText(file);
                var syntaxTree = CSharpSyntaxTree.ParseText(code);
                var root = syntaxTree.GetRoot();

                // Clássicos com [Http*]
                var methods = root.DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(m => m.AttributeLists.Any());

                foreach (var method in methods)
                {
                    var httpAttr = method.AttributeLists
                        .SelectMany(a => a.Attributes)
                        .FirstOrDefault(a => a.Name.ToString().StartsWith("Http"));

                    if (httpAttr != null)
                    {
                        var verb = httpAttr.Name.ToString().Replace("Http", "").ToLower();
                        var route = httpAttr.ArgumentList?.Arguments.FirstOrDefault()?.ToString().Trim('"') 
                                    ?? $"/{method.Identifier.Text.ToLower()}";

                        endpoints.Add(new SailEndpoint
                        {
                            Path = route,
                            Method = verb,
                            Intent = method.Identifier.Text.ToLower(),
                            Description = $"Auto-generated for {method.Identifier.Text}",
                            Meaning = "This was generated automatically by OpenSAIL.",
                            Examples = new[] { $"Call {method.Identifier.Text} via API" }
                        });
                    }
                }

                // Minimal APIs
                var invocations = root.DescendantNodes().OfType<InvocationExpressionSyntax>();

                foreach (var invocation in invocations)
                {
                    var expr = invocation.Expression.ToString();

                    if (expr.Contains("MapGet") || expr.Contains("MapPost") || expr.Contains("MapPut") || expr.Contains("MapDelete"))
                    {
                        var verb = expr.Replace("Map", "").ToLower();
                        var args = invocation.ArgumentList.Arguments;
                        if (args.Count == 0) continue;

                        var path = args[0].ToString().Trim('"');
                        var intent = $"minimal_{verb}_{path.Trim('/').Replace('/', '_')}";

                        endpoints.Add(new SailEndpoint
                        {
                            Path = path,
                            Method = verb,
                            Intent = intent,
                            Description = $"Auto-generated for {path}",
                            Meaning = "This was inferred from minimal API declaration.",
                            Examples = new[] { $"Example call for {path}" }
                        });
                    }
                }
            }

            return endpoints;
        }
    }
}