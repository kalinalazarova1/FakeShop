using Newtonsoft.Json;

namespace FakeShop.Models
{
    public class OrderDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

        [JsonProperty("postcode")]
        public string PostCode { get; set; }

        [JsonProperty("lines")]
        public List<OrderLineDocument> Lines { get; set; }
    }
}
