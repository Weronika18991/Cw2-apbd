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
            Console.WriteLine("Enter CSV file path!");
            string csvFilePath = Console.ReadLine();
            
            Console.WriteLine("Enter output file path!");
            string outputFilePath = Console.ReadLine();
            
            Console.WriteLine("Enter data format");
            string dataFormat = Console.ReadLine();
            
            var path = (String.IsNullOrEmpty(csvFilePath) ? @"dane.csv" : csvFilePath);
            var outputPath = (String.IsNullOrEmpty(outputFilePath) ? "result.xml" : outputFilePath);

            if (!dataFormat.Equals("xml"))
            {
                Console.WriteLine("Unsupported data type");
                Environment.Exit(1);
            }
            

            var students = new List<Student>();
            var incorrectData = new List<String>();
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

                        students.Add(new Student()
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
                        incorrectData.Add(lines[i]);
                    }
                }

                //removing duplicated students

                foreach (Student s1 in students)
                {
                    bool duplicate = false;
                    foreach (Student s2 in uniqueList)
                        if (s1.indexNumber == s2.indexNumber)
                        {
                            duplicate = true;
                            incorrectData.Add(s1.ToString());
                        }

                    if (!duplicate)
                        uniqueList.Add(s1);
                }

                //Counting students in a given field of study

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
                        name = i.Key,
                        numberOfStudents = i.Value
                    });
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Incorrect file path");
                incorrectData.Add(e.Message);
            }
            catch (FileNotFoundException f)
            {
                Console.WriteLine("File doesn't exists!");
                incorrectData.Add(f.Message);
            }
            

            FileStream writer = new FileStream(outputPath, FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(Uczelnia));
            
            Uczelnia uczelnia = new Uczelnia()
            {
                author = "Weronika Smardz",
                createdAt = DateTime.Today.ToShortDateString(),
                students = uniqueList,
                activeStudies = studies
            };

            serializer.Serialize(writer, uczelnia);
            writer.Dispose();
            
            using (StreamWriter file = new StreamWriter(@"log.txt"))
            {
                foreach (string line in incorrectData)
                {
                    file.WriteLine(line);
                }
            }


        }
    }
}
