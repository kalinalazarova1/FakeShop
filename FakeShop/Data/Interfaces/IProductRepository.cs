using FakeShop.Models;

namespace FakeShop.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDocument>> GetAsync();

        Task<ProductDocument> GetAsync(string productId);

        Task<IEnumerable<ProductDocument>> GetInStockAsync();

        Task<IEnumerable<ProductDocument>> GetFeaturedAsync();

        Task<ProductDocument> AddAsync(ProductDocument product);

        Task DeleteAsync(string id);

        Task<ProductDocument> UpdateAsync(ProductDocument product);
    }
}
