using FakeShop.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FakeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCart shoppingCart;
        private readonly IProductRepository productRepository;

        public ShoppingCartController(
            IShoppingCart shoppingCart,
            IProductRepository productRepository)
        {
            this.shoppingCart = shoppingCart;
            this.productRepository = productRepository;
        }

        [HttpPost("{productId}")]
        public async Task<ActionResult> AddToShoppingCart(string productId)
        {
            var selected = await productRepository.GetAsync(productId);

            if (selected != null)
            {
                await shoppingCart.AddToCartAsync(selected);
            }

            return Ok(await shoppingCart.GetAsync());
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> RemoveFromShoppingCart(string productId)
        {
            var selected = await productRepository.GetAsync(productId);

            if (selected != null)
            {
                await shoppingCart.RemoveFromCartAsync(selected);
            }

            return Ok(await shoppingCart.GetAsync());
        }

        [HttpDelete]
        public async Task<ActionResult> ClearShoppingCart()
        {
            await shoppingCart.ClearCartAsync();
            return NoContent();
        }
    }
}
