using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using OpenSAIL.Core.Filters;
using OpenSAIL.Core.Loaders;

namespace OpenSAIL.Core.Extensions;

public static class OpenApiAiExtensions
{
    public static IServiceCollection AddAiExtensions(this IServiceCollection services, string manifestPath = "sail.manifest.json")
    {
        var manifest = SailManifestLoader.Load(manifestPath);
        services.AddSingleton(manifest);

        services.Configure<SwaggerGenOptions>(options =>
        {
            options.OperationFilter<InjectAiMetadataFromManifest>();
        });

        return services;
    }
}