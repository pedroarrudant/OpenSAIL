using System.Collections.Generic;

namespace OpenSail.Core.Models
{
    public class EndpointSpecification
    {
        public string Path { get; set; } = string.Empty;
        public string Method { get; set; } = "get";
        public string Intent { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public List<string>? Examples { get; set; }
    }

    public class SailManifest
    {
        public List<EndpointSpecification> Endpoints { get; set; }
    }
}