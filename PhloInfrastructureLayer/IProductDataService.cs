using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhloInfrastructureLayer
{
    public interface IProductDataService
    {
        Task<List<Product>> GetProductDataAsync();
    }
}
