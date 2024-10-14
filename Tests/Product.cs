using PhloSystemController;

namespace Tests
{
    public class ProductTest
    {
        [Fact]
        public async void GetProducts_Should_Return_All_Products_When_No_Filters_Applied()
        {
            using (var httpClient = new HttpClient())
            {
                var product = new ProductService(httpClient);
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
    }
}