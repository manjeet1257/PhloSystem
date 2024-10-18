using PhloSystemDomain;

namespace PhloInfrastructureLayer
{
    public class ProductResponse
    {
        public List<PhloSystemDomain.Product> Products { get; set; }
        public ProductFilter ProductFilter { get; set; }
    }
}
