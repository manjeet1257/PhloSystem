using System.Text.Json.Serialization;

namespace PhloInfrastructureLayer
{
    public class ApiKeys
    {
        [JsonPropertyName("primary")]
        public string Primary { get; set; }

        [JsonPropertyName("secondary")]
        public string Secondary { get; set; }
    }
}
