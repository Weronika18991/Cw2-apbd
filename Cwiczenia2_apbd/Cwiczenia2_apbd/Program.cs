using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Cw2_apbd
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Podaj lokalizację pliku!");
            // string par1 = Console.ReadLine();
            //
            // Console.WriteLine("Podaj plik wyjściowy!");
            // string par2 = Console.ReadLine();
            //
            // var lokalizacja = (String.IsNullOrEmpty(par1) ? "dane.csv" : par1);
            // var output = (String.IsNullOrEmpty(par2) ? "result.xml" : par2);
            
            string path = @"Data\dane.csv";

            //Wczytywanie pliku
            var fi = new FileInfo(path);
            using (var stream = new StreamReader(fi.OpenRead()))
            {
                //kkjkjkjkjkj11
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }

            // stream.Dispose();

            //XML
            var list = new List<Student>();
            var st = new Student
            {
                Imie = "Jan",
                Nazwisko = "Kowalski",
                Email = "kowalski@wp.pl"
            };
            list.Add(st);

            FileStream writer = new FileStream(@"data.xml", FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(Uczelnia));
            
            Uczelnia uczelnia = new Uczelnia()
            {
                author = "Weronika Smardz",
                createdAt = DateTime.Today.ToShortDateString(),
                studenci = list
            };

            serializer.Serialize(writer, uczelnia);
            writer.Dispose();



        }
    }
}
