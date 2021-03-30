using System;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Purchase")]
    public class PurchaseExportDto
    {
        public string Card { get; set; }
        public string Cvc { get; set; }
        public string Date { get; set; }
        public PurchasedGameDto Game { get; set; }
        
       
    }
}