using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhloSystemController
{
    public class JsonResult
    {
        [JsonPropertyName("products")]
        public List<Product> Products { get; set; }

        [JsonPropertyName("apiKeys")]
        public ApiKeys ApiKeys { get; set; }
    }
}
