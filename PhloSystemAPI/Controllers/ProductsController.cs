using Microsoft.AspNetCore.Mvc;
using PhloInfrastructureLayer;
using PhloSystemDomain;

namespace PhloSystemAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<IProductService> _logger;

        public ProductsController(IProductService productService, ILogger<IProductService> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterProducts(
            [FromQuery] decimal? minprice,
            [FromQuery] decimal? maxprice,
            [FromQuery] string? size,
            [FromQuery] string? highlight)
        {
            // Input validation: Ensure valid prices
            if (minprice.HasValue && minprice < 0)
            {
                _logger.LogError("Minimum price cannot be negative.");
                return BadRequest("Minimum price cannot be negative.");
            }

            if (maxprice.HasValue && maxprice < 0)
            {
                _logger.LogError("Maximum price cannot be negative.");
                return BadRequest("Maximum price cannot be negative.");
            }

            if (minprice.HasValue && maxprice.HasValue && minprice > maxprice)
            {
                _logger.LogError("Minimum price cannot be greater than maximum price.");
                return BadRequest("Minimum price cannot be greater than maximum price.");
            }

            var Highlights = highlight?.Split(',')
                                    .Where(h => !string.IsNullOrWhiteSpace(h)) // Filter out empty values
                                    .Select(h => h.Trim())                    // Trim spaces
                                    .ToList() ?? new List<string>();

            var productResponse = await _productService.GetProductsAsync(minprice, maxprice, size, Highlights);

            return Ok(productResponse);
        }
    }
}
