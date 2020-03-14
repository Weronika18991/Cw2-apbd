using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cw2_apbd
{
   public class Student
    {
        [XmlAttribute("indexNumber")]
        public string indexNumber { get; set; }
        [XmlElement("fname")]
        public string fname { get; set; }
        [XmlElement("lname")]
        public string lname { get; set; }
        [XmlElement("birthdate")]
        public string birthdate { get; set; }
        [XmlElement("email")]
        public string email { get; set; }
        [XmlElement("mothersName")]
        public string mothersName { get; set; }
        [XmlElement("fathersName")]
        public string fathersName { get; set; }

        public Studies studies { get; set; }


        public override string ToString()
        {
            return fname + "," + lname + "," + studies.name + "," + studies.mode + "," + indexNumber + "," + birthdate +
                   "," + email + "," + mothersName + "," + fathersName;
        }


    }
}
