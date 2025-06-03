using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Text.Json;
using OpenSAIL.Cli.Models;

namespace OpenSAIL.Cli.Services;

public class ManifestGenerator
{
    public void Generate(FileInfo project, FileInfo output)
    {
        var assembly = BuildAssembly(project);

        var manifest = new SailManifest();

        foreach (var type in assembly.GetTypes())
        {
            if (!IsController(type))
                continue;

            var baseRoute = GetRouteTemplate(type);

            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!TryGetHttpInfo(method, out var verb, out var template))
                    continue;

                var path = CombineRoute(baseRoute, template);

                manifest.Endpoints.Add(new SailEndpoint
                {
                    Path = path,
                    Method = verb,
                    Intent = $"{verb}_{method.Name}".ToLowerInvariant(),
                    Description = $"Auto generated from {type.Name}.{method.Name}",
                    Meaning = string.Empty,
                    Examples = Array.Empty<string>()
                });
            }
        }

        var json = JsonSerializer.Serialize(manifest, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(output.FullName, json);
        Console.WriteLine($"Manifest generated at: {output.FullName}");
    }

    private static Assembly BuildAssembly(FileInfo project)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"build \"{project.FullName}\" -c Release",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        using var proc = Process.Start(psi) ?? throw new InvalidOperationException("Failed to start build process");
        proc.WaitForExit();

        if (proc.ExitCode != 0)
            throw new InvalidOperationException($"dotnet build failed: {proc.StandardError.ReadToEnd()}");

        var projectDir = project.Directory!.FullName;
        var dllName = Path.GetFileNameWithoutExtension(project.Name) + ".dll";
        var releaseDir = Path.Combine(projectDir, "bin", "Release");
        var tfmDir = Directory.GetDirectories(releaseDir).FirstOrDefault() ?? throw new FileNotFoundException("Could not determine target framework");
        var assemblyPath = Path.Combine(tfmDir, dllName);

        if (!File.Exists(assemblyPath))
            throw new FileNotFoundException($"Assembly not found at {assemblyPath}");

        return Assembly.LoadFrom(assemblyPath);
    }

    private static bool IsController(Type type)
    {
        if (!type.IsClass || type.IsAbstract)
            return false;

        if (type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
            return true;

        if (type.BaseType?.Name == "ControllerBase")
            return true;

        return type.GetCustomAttributes().Any(a => a.GetType().Name == "ApiControllerAttribute");
    }

    private static string? GetRouteTemplate(MemberInfo member)
    {
        var attr = member.GetCustomAttributes().FirstOrDefault(a => a.GetType().Name == "RouteAttribute");
        return attr?.GetType().GetProperty("Template")?.GetValue(attr)?.ToString();
    }

    private static bool TryGetHttpInfo(MethodInfo method, out string verb, out string? template)
    {
        var attr = method.GetCustomAttributes().FirstOrDefault(a => a.GetType().Name.StartsWith("Http") && a.GetType().Name.EndsWith("Attribute"));

        if (attr == null)
        {
            verb = string.Empty;
            template = null;
            return false;
        }

        var name = attr.GetType().Name;
        verb = name.Substring(4, name.Length - 13).ToLowerInvariant(); // trim 'Http' and 'Attribute'
        template = attr.GetType().GetProperty("Template")?.GetValue(attr)?.ToString();
        return true;
    }

    private static string CombineRoute(string? baseRoute, string? methodRoute)
    {
        baseRoute ??= string.Empty;
        methodRoute ??= string.Empty;

        var combined = $"{baseRoute}/{methodRoute}".Replace("//", "/");
        return "/" + combined.Trim('/');
    }
}
