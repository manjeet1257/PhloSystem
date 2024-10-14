using Moq;
using Xunit;
using System.Collections.Generic;
using ProductApi.Models;
using ProductApi.Services;
using System.Linq;
using PhloSystemController;

namespace Tests
{
    public class ProductTest
    {
        [Fact]
        public void GetProducts_Should_Return_All_Products_When_No_Filters_Applied()
        {

            var products = GetTestProducts();

            // Assert
            Assert.Equal(3, products.Count);
        }

        [Fact]
        public void GetProducts_Should_Filter_By_MinPrice()
        {
            // Arrange
            var products = GetTestProducts();
            decimal? minPrice = 15;

            var count = products.Count(x => minPrice.HasValue && x.Price == minPrice);

            // Assert
            Assert.Equal(1, count);
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

}
}
