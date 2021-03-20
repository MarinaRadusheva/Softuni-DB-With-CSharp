using System.Xml.Serialization;

namespace RealEstates.Services.Models
{
    [XmlType("Property")]
    public class TopFloorFullInfoDto
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }
        [XmlElement("DistrictName")]
        public string DistrictName { get; set; }
        [XmlElement("Year")]
        public int? Year { get; set; }
        [XmlElement("Size")]
        public int Size { get; set; }
        [XmlElement("Price")]
        public int? Price { get; set; }
        [XmlElement("Floor")]
        public byte? Floor { get; set; }
        [XmlElement("PropertyType")]
        public string PropertyType { get; set; }
        [XmlElement("BuildingTypeName")]
        public string BuildingTypeName { get; set; }
        [XmlArray("Tags")]
        public TagInfoDto[] Tags { get; set; }
    }
}