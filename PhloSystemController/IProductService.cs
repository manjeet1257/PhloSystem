using PhloInfrastructureLayer;

namespace PhloSystemDomain
{
    public interface IProductService
    {
        Task<ProductResponse> GetProductsAsync(decimal? minprice, decimal? maxprice, string? size, List<string> highlight);

        ProductResponse FilterProducts(List<ProductData> products, decimal? minprice, decimal? maxprice, string? size, List<string> highlight);
    }
}