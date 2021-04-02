using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Cinema.DataProcessor.ExportDto
{
    public class MovieJsonDto
    {
        public string MovieName { get; set; }
        public string Rating { get; set; }
        [JsonIgnore]
        public double RatingValue { get; set; }
        public string TotalIncomes { get; set; }
        public ICollection<CustomerJsonDto> Customers { get; set; }
    }
}
