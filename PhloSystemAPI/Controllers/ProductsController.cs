using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhloSystemController;
using System.Text.RegularExpressions;

namespace PhloSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterProducts(
            [FromQuery] decimal? minprice,
            [FromQuery] decimal? maxprice,
            [FromQuery] string? sizes,
            [FromQuery] string? highlight)
        {
            var products = await _productService.GetProductsAsync();

            return Ok(new { Products = products });
        }



    }
}
