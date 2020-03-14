using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cw2_apbd
{
    public class Uczelnia
    {
        [XmlAttribute("createdAt")]
        public string createdAt { get; set; }
        [XmlAttribute("author")]
        public string author { get; set; }

        public List<Student> studenci { get; set; }
        public List<Studies> activeStudies { get; set; }
    }
}