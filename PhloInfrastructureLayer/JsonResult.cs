using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhloInfrastructureLayer
{
    public class JsonResult
    {
        [JsonPropertyName("products")]
        public List<ProductData> ProductDatas { get; set; }
        [JsonPropertyName("apikeys")]
        public ApiKeys ApiKeys { get; set; }
    }
}
