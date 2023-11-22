using FakeShop.Data.Interfaces;
using FakeShop.Models;
using System.Security.Claims;

namespace FakeShop.Data
{
    public class ShoppingCart : IShoppingCart
    {
        private readonly IUserRepository userRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ShoppingCart(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task AddToCartAsync(ProductDocument product)
        {
            var email = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            var user = await userRepository.GetAsync(email);
            if(user.ShoppingCart.Any(i => i.Product.ProductId == product.ProductId))
            {
                var item = user.ShoppingCart.First(i => i.Product.ProductId == product.ProductId);
                item.Quantity++;
            }
            else
            {
                user.ShoppingCart.Add(new ShoppingCartItemDocument
                {
                    Product = product,
                    Quantity = 1
                });
            }

           await userRepository.UpdateAsync(user);
        }

        public async Task ClearCartAsync()
        {
            var email = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            var user = await userRepository.GetAsync(email);
            user.ShoppingCart = new List<ShoppingCartItemDocument>();
        }

        public async Task<List<ShoppingCartItemDocument>> GetAsync()
        {
            var email = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            var user = await userRepository.GetAsync(email);

            return user.ShoppingCart;
        }

        public async Task RemoveFromCartAsync(ProductDocument product)
        {
            var email = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            var user = await userRepository.GetAsync(email);
            var removeItem = user.ShoppingCart.FirstOrDefault(i => i.Product.ProductId == product.ProductId);
            if (removeItem != null)
            {
                user.ShoppingCart.Remove(removeItem);
                await userRepository.UpdateAsync(user);
            }
        }
    }
}
