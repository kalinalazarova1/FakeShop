using FakeShop.Models;

namespace FakeShop.Data.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<OrderDocument>> GetAsync();

        Task<OrderDocument> AddAsync(AddressModel address);
    }
}
