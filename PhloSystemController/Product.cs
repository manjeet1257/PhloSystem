using System.Text.Json;
using System.Text.Json.Serialization;

namespace PhloSystemController
{
    public class Product
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("price")]
        public int? Price { get; set; }

        [JsonPropertyName("sizes")]
        public List<string> Sizes { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    


}
