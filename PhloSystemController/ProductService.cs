using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using PhloInfrastructureLayer;

namespace PhloSystemDomain
{
    public class ProductService : IProductService

    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductDataService _productDataService;

        public ProductService(ILogger<ProductService> logger, IProductDataService productDataService)
        {
            _logger = logger;
            _productDataService = productDataService;
        }

        public async Task<ProductResponse> GetProductsAsync(decimal? minprice, decimal? maxprice, string? size, List<string> highlight)
        {
            var productDataList = await _productDataService.GetProductDataAsync();
            var productResponse = FilterProducts(productDataList, minprice, maxprice, size, highlight);

            return productResponse;
        }

        private ProductResponse FilterProducts(List<ProductData> products, decimal? minprice, decimal? maxprice, string? size, List<string> highlight)
        {
            //Filtering
            var minimumPrice = decimal.MaxValue; var maximumPrice = decimal.MinValue;
            var productList = new List<Product>();

            foreach (var product in products)
            {
                if (product.Price.HasValue) // Ensure Price is not null
                {
                    minimumPrice = Math.Min(product.Price.Value, minimumPrice);
                    maximumPrice = Math.Max(product.Price.Value, maximumPrice);

                    // Check if the product falls within the price range
                    bool meetsMinPrice = !minprice.HasValue || product.Price >= minprice.Value;
                    bool meetsMaxPrice = !maxprice.HasValue || product.Price <= maxprice.Value;

                    // Add the product if it meets both criteria (within min and max range)
                    if (meetsMinPrice && meetsMaxPrice)
                    {
                        productList.Add(new Product
                        {
                            Description = product.Description,
                            Price = product.Price,
                            Sizes = product.Sizes,
                            Title = product.Title
                        });
                    }
                }
            }

            if (!string.IsNullOrEmpty(size))
                productList = productList.Where(p => p.Sizes.Any(x => x.Equals(size, StringComparison.OrdinalIgnoreCase))).ToList();

            // Highlighting words in description
            if (highlight.Count > 0)
            {
                productList.ForEach(product =>
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

            var filterProducts = new ProductFilter();
            filterProducts.MinPrice = minimumPrice;
            filterProducts.MaxPrice = maximumPrice;
            filterProducts.Sizes = products.SelectMany(p => p.Sizes).Distinct().ToList();
            filterProducts.CommonWords = GetMostCommonWords(products, 10, 5);

            return new ProductResponse { ProductFilter = filterProducts, Products = productList };
        }

        private List<string> GetMostCommonWords(List<ProductData> products, int topCount, int excludeTop)
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
