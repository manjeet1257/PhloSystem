namespace PhloInfrastructureLayer
{
    public interface IProductDataService
    {
        Task<List<ProductData>> GetProductDataAsync();
    }
}
