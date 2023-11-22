using FakeShop.Models;

namespace FakeShop.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDocument> GetAsync(string email);

        Task<UserDocument> AddAsync(UserDocument user);

        Task<UserDocument> UpdateAsync(UserDocument user);
    }
}
