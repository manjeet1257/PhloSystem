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

        public List<Product> FilterProducts(List<Product> products, ProductFilter filterProducts, decimal? minprice, decimal? maxprice,string? size, List<string> highlight)
        {
            // Filtering
            if (minprice.HasValue)
                products = products.Where(p => p.Price >= minprice).ToList();

            if (maxprice.HasValue)
                products = products.Where(p => p.Price >= maxprice).ToList();

            if (!string.IsNullOrEmpty(size))
                products = products.Where(p => p.Sizes.Any(x => x.Equals(size, StringComparison.OrdinalIgnoreCase))).ToList();

            // Highlighting words in description
            if (highlight.Count > 0)
            {
                products.ForEach(product =>
                {
                    foreach (var word in highlight)
                    {
                        if (product.Description.Contains(word, StringComparison.OrdinalIgnoreCase))
                        {
                            product.Description = Regex.Replace(product.Description,
                                word, $"<em>{word}</em>", RegexOptions.IgnoreCase);
                        }
                    }
                });
            }


            filterProducts.MinPrice = products.Min(p => p.Price);
            filterProducts.MaxPrice = products.Max(p => p.Price);
            filterProducts.Sizes = products.SelectMany(p => p.Sizes).Distinct().ToList();
            filterProducts.CommonWords = GetMostCommonWords(products, 10, 5);

            return products;
        }

        private List<string> GetMostCommonWords(List<Product> products, int topCount, int excludeTop)
        {
            var allDescriptions = string.Join(" ", products.Select(p => p.Description));
            var wordFrequency = Regex.Matches(allDescriptions.ToLower(), @"\w+")
                                      .Cast<Match>()
                                      .GroupBy(m => m.Value)
                                      .ToDictionary(g => g.Key, g => g.Count());

            // Order by frequency, remove top common words, and return top N after excluding the most common 5
            var topWords = wordFrequency.OrderByDescending(kv => kv.Value)
                                        .Skip(excludeTop) // Skip the most common 'excludeTop' words
                                        .Take(topCount)
                                        .Select(kv => kv.Key)
                                        .ToList();

            return topWords;
        }

    }
}
