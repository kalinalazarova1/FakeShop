using Newtonsoft.Json;

namespace FakeShop.Models
{
    public class ProductDocument
    {
        [JsonProperty("productid")]
        public string ProductId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("imageurl")]
        public string ImageUrl { get; set; }

        [JsonProperty("instock")]
        public bool IsInStock { get; set; }

        [JsonProperty("featured")]
        public bool Featured { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; } = new List<string>();
    }
}
