using Newtonsoft.Json;

namespace Cinema.DataProcessor.ExportDto
{
    public class CustomerJsonDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Balance { get; set; }
        [JsonIgnore]
        public decimal  BalanceDec { get; set; }
    }
}