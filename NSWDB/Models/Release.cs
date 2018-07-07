// Generated using http://xmltocsharp.azurewebsites.net/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace NSWDB.Models
{
    [XmlRoot(ElementName = "release")]
    public class Release
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "publisher")]
        public string Publisher { get; set; }
        [XmlElement(ElementName = "region")]
        public string Region { get; set; }
        [XmlElement(ElementName = "languages")]
        public string Languages { get; set; }
        [XmlElement(ElementName = "group")]
        public string Group { get; set; }
        [XmlElement(ElementName = "imagesize")]
        public string ImageSize { get; set; }
        [XmlElement(ElementName = "serial")]
        public string Serial { get; set; }
        [XmlElement(ElementName = "titleid")]
        public string TitleId { get; set; }
        [XmlElement(ElementName = "imgcrc")]
        public string ImgCRC { get; set; }
        [XmlElement(ElementName = "filename")]
        public string FileName { get; set; }
        [XmlElement(ElementName = "releasename")]
        public string ReleaseName { get; set; }
        [XmlElement(ElementName = "trimmedsize")]
        public string TrimmedSize { get; set; }
        [XmlElement(ElementName = "firmware")]
        public string Firmware { get; set; }
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "card")]
        public string Card { get; set; }
    }

    [XmlRoot(ElementName = "releases")]
    public class ReleasesRoot
    {
        [XmlElement(ElementName = "release")]
        public List<Release> Release { get; set; }
    }
}
