using System.Xml.Serialization;

namespace Cw2_apbd
{
    public class Studies
    {
        [XmlElement("name")]
        public string name { get; set; }

        public bool ShouldSerializenazwa() {
            return string.IsNullOrEmpty(name);
        }
        public string mode { get; set; }
        [XmlAttribute("numberOfStudents")]

        public int numberOfStudents { get; set; }

        public bool ShouldSerializenumberOfStudents()
        {
            return numberOfStudents > 0;
        }
        
    }
}