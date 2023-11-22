using FakeShop.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FakeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository; 
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await productRepository.GetAsync());
        }
    }
}
