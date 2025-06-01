using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Any;
using OpenSail.Core.Models;

namespace OpenSAIL.Core.Filters
{
    public class InjectAiMetadataFromManifest : IOperationFilter
    {
        private readonly SailManifest _manifest;

        public InjectAiMetadataFromManifest(SailManifest manifest)
        {
            _manifest = manifest;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var path = "/" + context.ApiDescription.RelativePath?.Trim('/');
            var method = context.ApiDescription.HttpMethod?.ToLower();

            var match = _manifest.Endpoints
                .FirstOrDefault(e => e.Path.Equals(path, StringComparison.OrdinalIgnoreCase)
                                     && e.Method.Equals(method, StringComparison.OrdinalIgnoreCase));
        
            if (match == null) return;

            operation.Extensions.TryAdd("x-ai-intent", new OpenApiString(match.Intent));
            operation.Extensions.TryAdd("x-ai-description", new OpenApiString(match.Description));
            operation.Extensions.TryAdd("x-ai-meaning", new OpenApiString(match.Meaning));

            if (match.Examples?.Any() == true)
            {
                var array = new OpenApiArray();
                foreach (var ex in match.Examples)
                    array.Add(new OpenApiString(ex));
                operation.Extensions.TryAdd("x-ai-examples", array);
            }
        }
    }
}