using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DataTransferObjects
{
    public class UsersWithSoldProductsModel
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("soldProducts")]
        public ICollection<ProductsSoldModel> SoldProducts { get; set; }
    }
}
