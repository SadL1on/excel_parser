using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace excel_parser
{
    class Program
    {
        public static string[] arr = new string[8] { "~р-он", "~р-н", "~р/н", "~г.", "~с.", "~п.г.т.", "~р.п.", "~п." };

        static void Main(string[] args)
        {
            List<string> DBList = DB_Reader();
            DataReader(DBList);
        }

        public static void DataReader(List<string> DBList)
        {
            string pathData = @"C:\Users\deamo\Desktop\rtt8.txt";
            string Output = string.Empty;
            using (StreamReader sr = new StreamReader(pathData))
            {
                string lineData;
                while ((lineData = sr.ReadLine()) != "~")
                {
                    Output = Shelud(lineData, DBList);
                }
            }
        }

        public static string Shelud(string line, List<string> DBList)
        {
            var result = string.Empty;
            string[] words = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = "~" + words[i];
            }

            for (int i = 0; i < words.Length; i++)
            {
                Console.WriteLine(words[i]);
                //Поиск по элементу из массива arr
                var str = arr.FirstOrDefault(x => words[i].Contains(x));
                if (str != null)
                {
                    result = words[i].Substring(1);
                    if (str == arr[0] || str == arr[1] || str == arr[2])
                    {
                        result = words[i - 1].Substring(1) + " " + result + " " + words[i + 1].Substring(1);
                    }
                    Console.WriteLine("!!!!!!!");
                    break;
                }
                //Поиск по наименованию из БД DBList
                else
                {
                    var str2 = DBList.FirstOrDefault(x => words[i].Contains(x));
                    if (str2 != null)
                    {
                        result = words[i].Substring(1);
                        Console.WriteLine("!!!");
                        break;
                    }
                    else { Console.WriteLine("Неизвестно"); }
                }
            }

            Console.WriteLine();
            Console.WriteLine(result);
            Console.ReadLine();
            return result;
        }

        public static List<string> DB_Reader()
        {
            string pathBD = @"C:\Users\deamo\Desktop\BD.txt";
            string[] mass = new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9", };
            using (StreamReader reader = new StreamReader(pathBD))
            {
                string lineDB;
                List<string> DBList = new List<string>();
                while ((lineDB = reader.ReadLine()) != "~")
                {
                    var str = mass.FirstOrDefault(x => lineDB.Contains(x));
                    if (str != null) { continue; }
                    string[] NewLine = lineDB.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    DBList.AddRange(NewLine);
                }
                for (int i = 0; i < DBList.Count; i++)
                {
                    DBList[i] = "~" + DBList[i];
                }
                return DBList;
            }
        }
    }
}