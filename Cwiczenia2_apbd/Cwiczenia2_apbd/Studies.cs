using System.Xml.Serialization;

namespace Cw2_apbd
{
    public class Studies
    {
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("mode")]
        public string mode { get; set; }

        public int numberOfStudents { get; set; }

        public bool ShouldSerializenumberOfStudents()
        {
            return numberOfStudents > 0;
        }
        
    }
}