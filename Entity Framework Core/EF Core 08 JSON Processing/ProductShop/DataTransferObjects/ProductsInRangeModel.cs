﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DataTransferObjects
{
    public class ProductsInRangeModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("seller")]
        public string SellerFullName { get; set; }
    }
}
