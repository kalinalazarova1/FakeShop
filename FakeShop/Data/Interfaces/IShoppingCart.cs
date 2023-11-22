using FakeShop.Models;

namespace FakeShop.Data.Interfaces
{
    public interface IShoppingCart
    {
        Task<List<ShoppingCartItemDocument>> GetAsync();

        Task AddToCartAsync(ProductDocument product);

        Task RemoveFromCartAsync(ProductDocument product);

        Task ClearCartAsync();

    }
}
