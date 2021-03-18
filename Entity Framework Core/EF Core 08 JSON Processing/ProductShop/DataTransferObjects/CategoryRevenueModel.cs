using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DataTransferObjects
{
    public class CategoryRevenueModel
    {
        [JsonProperty("category")]
        public string Name { get; set; }
        [JsonProperty("productsCount")]
        //[IgnoreDataMember]
        //public decimal Amount { get; set; }

        //[DataMember(Name = "Amount")]
        //public decimal RoundedAmount { get { return Math.Round(Amount, 2); } }
        public int ProductCount { get; set; }

        [JsonIgnore]
        public decimal AvgeragePrice { get; set; }

        [JsonProperty("averagePrice")]
        public string AvgeragePriceRounded => $"{this.AvgeragePrice:F2}";

        [JsonIgnore]
        public decimal TotalRevenue { get; set; }

        [JsonProperty("totalRevenue")]
        public string TotalRevenueRounded => $"{this.TotalRevenue:F2}";

    }
}
