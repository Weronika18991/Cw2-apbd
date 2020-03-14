using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Mime;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;

namespace Cw2_apbd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Podaj adres pliku CSV!");
            string v1 = Console.ReadLine();
            
            Console.WriteLine("Podaj adres pliku wyjsciowego!");
            string v2 = Console.ReadLine();
            
            Console.WriteLine("Podaj format danych");
            string format = Console.ReadLine();
            
            var path = (String.IsNullOrEmpty(v1) ? @"dane.csv" : v1);
            var outputPath = (String.IsNullOrEmpty(v2) ? "result.xml" : v2);

            if (!format.Equals("xml"))
            {
                Console.WriteLine("Nieobslugiwany typ danych");
                Environment.Exit(1);
            }
            

            var list = new List<Student>();
            var wrongData = new List<String>();
            Dictionary<string,int> map = new Dictionary<string, int>();
            List<Studies> studies = new List<Studies>();
            List<Student> uniqueList = new List<Student>();

            try
            {
                var lines = File.ReadAllLines(path);

                for (int i = 0; i < lines.Length; i++)
                {
                    int count = 0;
                    var line = lines[i].Split(",");
                    for (int j = 0; j < line.Length; j++)
                    {
                        if (String.IsNullOrEmpty(line[j]))
                            count++;
                    }

                    if (count == 0)
                    {
                        Studies s = new Studies()
                        {
                            name = line[2],
                            mode = line[3]
                        };

                        list.Add(new Student()
                        {
                            fname = line[0],
                            lname = line[1],
                            indexNumber = line[4],
                            birthdate = line[5],
                            email = line[6],
                            mothersName = line[7],
                            fathersName = line[8],
                            studies = s
                        });
                    }
                    else
                    {
                        wrongData.Add(lines[i]);
                    }
                }

                //usuwanie powtarzajacych sie studentow

                foreach (Student s1 in list)
                {
                    bool duplicate = false;
                    foreach (Student s2 in uniqueList)
                        if (s1.indexNumber == s2.indexNumber)
                        {
                            duplicate = true;
                            wrongData.Add(s1.ToString());
                        }

                    if (!duplicate)
                        uniqueList.Add(s1);
                }

                //zliczanie studentow na danym kierunku

                for (int i = 0; i < uniqueList.Count; i++)
                {
                    if (map.ContainsKey(uniqueList[i].studies.name))
                        map[uniqueList[i].studies.name]++;

                    if (!map.ContainsKey(uniqueList[i].studies.name))
                        map.Add(uniqueList[i].studies.name, 1);
                }

                foreach (var i in map)
                {
                    Console.WriteLine(i.Key + " " + i.Value);
                    studies.Add(new Studies()
                    {
                        nazwa = i.Key,
                        numberOfStudents = i.Value
                    });
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Podana sciezka jest niepoprawna");
                wrongData.Add(e.Message);
            }
            catch (FileNotFoundException f)
            {
                Console.WriteLine("Plik nie istnieje!");
                wrongData.Add(f.Message);
            }
            

            FileStream writer = new FileStream(outputPath, FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(Uczelnia));
            
            Uczelnia uczelnia = new Uczelnia()
            {
                author = "Weronika Smardz",
                createdAt = DateTime.Today.ToShortDateString(),
                studenci = uniqueList,
                activeStudies = studies
            };

            serializer.Serialize(writer, uczelnia);
            writer.Dispose();

            
            using (StreamWriter file = new StreamWriter(@"log.txt"))
            {
                foreach (string line in wrongData)
                {
                    file.WriteLine(line);
                }
            }


        }
    }
}
