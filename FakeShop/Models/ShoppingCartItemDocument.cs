using Newtonsoft.Json;

namespace FakeShop.Models
{
    public class ShoppingCartItemDocument
    {
        [JsonProperty("product")]
        public ProductDocument Product { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
