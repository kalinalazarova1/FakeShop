using FakeShop.Data.Interfaces;
using FakeShop.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace FakeShop.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IUserRepository userRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public OrderRepository(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<OrderDocument> AddAsync(AddressModel address)
        {
            var email = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            var user = await userRepository.GetAsync(email);
            var order = new OrderDocument 
            {
                Id = Guid.NewGuid().ToString(),
                Address1 = address.Address1,
                Address2 = address.Address2,
                PostCode = address.PostCode,
                Lines = user.ShoppingCart.Select(i => new OrderLineDocument 
                {
                    Product = i.Product,
                    Quantity = i.Quantity,
                    Price = i.Product.Price
                }).ToList()
            };

            user.ShoppingCart = new List<ShoppingCartItemDocument>();
            user.Orders.Add(order);
            await userRepository.UpdateAsync(user);
            return order;
        }

        public async Task<List<OrderDocument>> GetAsync()
        {
            var email = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            var user = await userRepository.GetAsync(email);
            return user.Orders;
        }
    }
}
