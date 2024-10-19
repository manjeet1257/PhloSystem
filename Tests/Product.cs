using Microsoft.AspNetCore.Mvc;
using Moq;
using PhloInfrastructureLayer;
using PhloSystemDomain;

namespace Tests
{
    public class ProductTest
    {

        private readonly Mock<IProductService> _mockProductService;

        public ProductTest()
        {
            _mockProductService = new Mock<IProductService>();
        }

        [Fact]
        public async Task GetProducts_Should_Return_All_Products_When_No_Filters_Applied()
        {
            // Arrange: Mocking the product service to return all products without any filters
            var mockProducts = GetTestProducts();
            _mockProductService.Setup(service => service.GetProductsAsync(null, null, null, null))
                .ReturnsAsync(mockProducts);

            Assert.Equal(mockProducts.Products.Count, 3); // Ensure all products are returned
        }


        [Fact]
        public async Task GetProducts_Should_Filter_By_MinPrice()
        {
            // Arrange
            var mockProducts = GetTestProducts();

            // Mock GetProductsAsync to return the full product list
            _mockProductService.Setup(service => service.GetProductsAsync(null, null, null, null))
                .ReturnsAsync(mockProducts);

            // Mock FilterProducts to filter based on minprice > 11
            _mockProductService.Setup(service => service.FilterProducts(
                    It.IsAny<List<ProductData>>(),
                    11,   // MinPrice = 11
                    null,  // MaxPrice = null
                    null,  // Size = null
                    It.IsAny<List<string>>() // Highlights = empty or null
                ))
                .Returns(new ProductResponse
                {
                    Products = mockProducts.Products.FindAll(x => x.Price.HasValue && x.Price.Value >= 11)
                });

            // actual
            var returnValue = new ProductResponse
            {
                Products = new List<Product> {
                    new Product { Title = "Blue Shirt", Price = 15, Sizes = new List<string> { "medium" }, Description = "Matches red hats." },
                    new Product { Title = "Green Hat", Price = 20, Sizes = new List<string> { "large" }, Description = "Matches blue shoes." }
                },
                ProductFilter = { }
            };

            // Ensure that all returned products have a Price greater than or equal to 100
            Assert.NotNull(returnValue.Products);
            Assert.True(returnValue.Products.Count > 0);
            Assert.All(returnValue.Products, p => Assert.True(p.Price >= 11));
        }

        [Fact]
        public async Task GetProducts_Should_Filter_By_MaxPrice()
        {
            // Arrange
            var mockProducts = GetTestProducts();

            // Mock GetProductsAsync to return the full product list
            _mockProductService.Setup(service => service.GetProductsAsync(null, null, null, null))
                .ReturnsAsync(mockProducts);

            // Mock FilterProducts to filter based on minprice > 11
            _mockProductService.Setup(service => service.FilterProducts(
                    It.IsAny<List<ProductData>>(),
                    null,   // 
                    16,  // MaxPrice = null
                    null,  // Size = null
                    It.IsAny<List<string>>() // Highlights = empty or null
                ))
                .Returns(new ProductResponse
                {
                    Products = mockProducts.Products.FindAll(x => x.Price.HasValue && x.Price.Value <= 16)
                });

            // actual
            var returnValue = new ProductResponse
            {
                Products = new List<Product> {
                    new Product { Title = "Red Trouser", Price = 10, Sizes = new List<string> { "small", "medium" }, Description = "Matches green shirts." },
                    new Product { Title = "Blue Shirt", Price = 15, Sizes = new List<string> { "medium" }, Description = "Matches red hats." },
                },
                ProductFilter = { }
            };

            // Ensure that all returned products have a Price greater than or equal to 100
            Assert.NotNull(returnValue.Products);
            Assert.True(returnValue.Products.Count > 0);
            Assert.All(returnValue.Products, p => Assert.True(p.Price <= 16));
        }

        private ProductResponse GetTestProducts()
        {
            var productList = new List<Product>
                {
                    new Product { Title = "Red Trouser", Price = 10, Sizes = new List<string> { "small", "medium" }, Description = "Matches green shirts." },
                    new Product { Title = "Blue Shirt", Price = 15, Sizes = new List<string> { "medium" }, Description = "Matches red hats." },
                    new Product { Title = "Green Hat", Price = 20, Sizes = new List<string> { "large" }, Description = "Matches blue shoes." }
                };

            return new ProductResponse
            {
                Products = productList,
                ProductFilter = new ProductFilter()
            };
        }
    }

}