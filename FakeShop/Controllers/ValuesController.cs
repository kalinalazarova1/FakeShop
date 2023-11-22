using FakeShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace FakeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ITestModel model;
        public ValuesController(ITestModel model)
        {
            this.model = model;
        }
        [HttpGet]
        public ActionResult Test()
        {
            return Ok(model.Replaced);
        }
    }
}
