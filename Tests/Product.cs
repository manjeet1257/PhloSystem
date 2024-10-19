using Microsoft.Extensions.Logging;
using Moq;
using PhloSystemAPI.Controllers;
using PhloSystemDomain;

namespace Tests
{
    public class ProductTest
    {

        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<IProductFilter> _mockProductFilter;
        private readonly Mock<ILogger<IProductService>> _mockLogger;

        public ProductTest()
        {
            _mockProductService = new Mock<IProductService>();
            _mockProductFilter = new Mock<IProductFilter>();
            _mockLogger = new Mock<ILogger<IProductService>>();

            // Initialize the controller with mock services
            _controller = new ProductsController(_mockProductService.Object, _mockProductFilter.Object, _mockLogger.Object);
        }

        [Fact]
        public async void GetProducts_Should_Return_All_Products_When_No_Filters_Applied()
        {
            using (var httpClient = new HttpClient())
            {
                var _logger = new ILogger<ProductService>();
                var product = new ProductService(httpClient, _logger);
                var productList = await product.GetProductsAsync();

                // Assert
                Assert.Equal(48, productList.Count);
            }
        }

        [Fact]
        public async void GetProducts_Should_Filter_By_MinPrice()
        {
            // Arrange
            using (var httpClient = new HttpClient())
            {
                var product = new ProductService(httpClient);
                var productList = await product.GetProductsAsync();
                decimal minPrice = 14;

                var count = productList.Count(x => x.Price == minPrice);

                // Assert
                Assert.Equal(3, count);
            }
        }

        [Fact]
        public async void GetProducts_Should_Filter_By_MaxPrice()
        {
            using (var httpClient = new HttpClient())
            {
                var product = new ProductService(httpClient);
                var productList = await product.GetProductsAsync();
                decimal maxPrice = 205;

                var count = productList.Count(x => x.Price == maxPrice);

                // Assert
                Assert.Equal(0, count);
            }
        }

        [Fact]
        public async void GetCommonWordsCount()
        {
            var products = GetTestProducts();
            var filter = new ProductFilter { };

            using (var httpClient = new HttpClient())
            {
                var product = new ProductService(httpClient);
                var productList = await product.GetProductsAsync();

                var filterPoruducts = product.FilterProducts(productList, filter, 15, 20, "small", new List<string>());
                Assert.NotEmpty(filter.CommonWords);
            }
        }

        private List<Product> GetTestProducts()
        {
            return new List<Product>
            {
                new Product { Title = "Red Trouser", Price = 10, Sizes = new List<string> { "small", "medium" }, Description = "Matches green shirts." },
                new Product { Title = "Blue Shirt", Price = 15, Sizes = new List<string> { "medium" }, Description = "Matches red hats." },
                new Product { Title = "Green Hat", Price = 20, Sizes = new List<string> { "large" }, Description = "Matches blue shoes." }
            };
        }

    }
}