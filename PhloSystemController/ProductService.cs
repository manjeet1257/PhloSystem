using System.Drawing;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PhloSystemController
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var url = "https://pastebin.com/raw/JucRNpWs";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(json))
                    return new List<Product>();

                var result = JsonSerializer.Deserialize<JsonResult>(json);

                return result == null ? new List<Product>() : result.Products;
            }

            return new List<Product>();
        }
    }
}
