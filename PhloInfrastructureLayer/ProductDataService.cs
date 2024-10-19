using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace PhloInfrastructureLayer
{
    public class ProductDataService : IProductDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductDataService> _logger;
        public ProductDataService(HttpClient httpClient, ILogger<ProductDataService> logger)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(20); // Set timeout to avoid long-hanging requests
            _logger = logger;
        }
        public async Task<List<ProductData>> GetProductDataAsync()
        {
            var url = "https://pastebin.com/raw/JucRNpWs";
            var response = await _httpClient.GetAsync(url);
            _logger.LogInformation($"Fetching products from {url}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(json))
                {
                    _logger.LogWarning("Response from API is empty or null");
                    return new List<ProductData>();
                }

                _logger.LogInformation("Received successful response from API");
                var result = JsonSerializer.Deserialize<JsonResult>(json);

                if (result == null)
                {
                    _logger.LogError("Failed to deserialize API response into Product list");
                    return new List<ProductData>();
                }

                _logger.LogInformation("Successfully deserialized API response");
                return result.ProductDatas;
            }
            else
                _logger.LogError($"Failed to fetch products. Status Code: {response.StatusCode}");

            return new List<ProductData>();
        }
    }
}
