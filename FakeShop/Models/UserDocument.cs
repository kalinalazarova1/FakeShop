using Newtonsoft.Json;

namespace FakeShop.Models
{
    public class UserDocument
    {
        [JsonProperty("userid")]
        public string UserId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("passwordhash")]
        public string PasswordHash { get; set; }

        [JsonProperty("shoppingcart")]
        public List<ShoppingCartItemDocument> ShoppingCart { get; set; } = new List<ShoppingCartItemDocument>();

        [JsonProperty("orders")]
        public List<OrderDocument> Orders { get; set; } = new List<OrderDocument>();
    }
}
