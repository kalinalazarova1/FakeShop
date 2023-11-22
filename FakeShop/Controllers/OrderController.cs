using FakeShop.Data.Interfaces;
using FakeShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FakeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await orderRepository.GetAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(AddressModel address)
        {
            return Ok(await orderRepository.AddAsync(address));
        }
    }
}
