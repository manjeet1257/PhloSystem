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
            var productList = new List<Product>();

            //We can use automapper here as well for conversion of one data type to another.
            foreach (var productData in productDataList)
                productList.Add(new Product { Description = productData.Description, Price = productData.Price, Sizes = productData.Sizes, Title = productData.Title });

            var _productFilter = new ProductFilter();
            var productResponse = FilterProducts(productList, _productFilter, minprice, maxprice, size, highlight);

            return productResponse;
        }

        private ProductResponse FilterProducts(List<Product> products, ProductFilter filterProducts, decimal? minprice, decimal? maxprice, string? size, List<string> highlight)
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

            return new ProductResponse { ProductFilter = filterProducts, Products = products };
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
