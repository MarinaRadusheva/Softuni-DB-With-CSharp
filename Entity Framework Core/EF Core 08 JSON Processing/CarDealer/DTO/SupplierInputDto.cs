using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CarDealer.DTO
{
    public class SupplierInputDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("isImporter")]
        public bool IsImporter { get; set; }
    }
}
