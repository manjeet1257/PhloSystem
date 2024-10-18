using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhloInfrastructureLayer
{
    public class Product
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("price")]
        public int? Price { get; set; }

        [JsonPropertyName("sizes")]
        public List<string> Sizes { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
