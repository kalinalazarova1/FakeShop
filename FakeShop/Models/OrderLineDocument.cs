﻿using Newtonsoft.Json;

namespace FakeShop.Models
{
    public class OrderLineDocument
    {
        [JsonProperty("product")]
        public ProductDocument Product { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
