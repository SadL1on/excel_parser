﻿using System;
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
        public static string[] arrDis = new string[4] { "~р-он", "~р-н", "~р/н", "~район" };
        public static string[] arrNP = new string[8] { "~г.", "~с.", "~п.г.т.", "~р.п.", "~п.", "~пос.", "~пгт.", "~пгт" };
        public static int A = 0;

        static void Main(string[] args)
        {
            List<string> DBList = DB_Reader();
            DataReader(DBList);
        }

        public static void DataReader(List<string> DBList)
        {
            string pathData = @"D:\KursRep\excel_parser\rtt8.txt";
          //  string pathData = @"D:\KursRep\excel_parser\test.txt";
            string pathToSave = @"D:\KursRep\excel_parser\out8.txt";
            string Output = string.Empty;
            var results = new List<string>();
            using (StreamReader sr = new StreamReader(pathData))
            {
                string lineData;
                while ((lineData = sr.ReadLine()) != "~")
                {
                    var dis = SearchDistrict(lineData);
                    var settl = SearchSettlement(lineData, DBList);
                    var result = dis + " " + settl;
                 //   Console.WriteLine(lineData);
                   // Console.WriteLine();
                    Console.WriteLine(result + " " + A);
               //     Console.ReadLine();
                    results.Add(result);
                }
            }
            using (StreamWriter sr = new StreamWriter(pathToSave))
            {
                foreach (var result in results)
                    sr.WriteLine(result);
            }
        }

        private static string[] LineHandler(string line, char ch = ' ')
        {
            string[] words = line.Split(new char[] { ch, ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = "~" + words[i];
            }
            return words;
        }

        private static string SearchDistrict(string line)
        {
            var result = string.Empty;
            var words = LineHandler(line);

            for (int i = 0; i < words.Length; i++)
            {
                var str0 = arrDis.FirstOrDefault(x => x == words[i]);
                if (str0 != null)
                {
                    
                    result = words[i].Substring(1);

                    if (str0 == arrDis[0] || str0 == arrDis[1] || str0 == arrDis[2] || str0 == arrDis[3])
                    {
                        if (i != 0)
                            result = words[i - 1].Substring(1) + " " + result;// + " " + words[i + 1].Substring(1);
                        else if (words.Length > 2)
                            result = result + " " + words[i + 1].Substring(1);
                        else { break; }
                        //try { if (words[i + 2].Substring(1).Take(1).All(Char.IsUpper) && words[i + 2].Substring(2).Take(1).All(Char.IsLower)) { result = result + " " + words[i + 2].Substring(1); } }
                        //catch { break; }
                    }
                    break;
                }
            }
            
            return result;
        }

        private static string SearchSettlement(string line, List<string> DBList)
        {
            var result = string.Empty;
            var str2 = string.Empty;
            var str1 = string.Empty;
            var str = string.Empty;
            var words = LineHandler(line);
            var wordsDB = LineHandler(line, '.');

            //Поиск по наименованию из БД DBList
            for(int i = 0; i < wordsDB.Length; i++)
            {
                str2 = DBList.FirstOrDefault(x => x == wordsDB[i]);              
                if (str2 == null) str2 = string.Empty;
                else { str2 = str2.Substring(1); }
            }
                        
            for (int i = 0; i < words.Length; i++)
            {
                str = arrNP.FirstOrDefault(x => words[i].Contains(x));
                if (str != null && words[i].Length > 4)
                {
                    result = words[i].Substring(1);
                    try
                    {
                        if (words[i + 1].Substring(1).Take(1).All(Char.IsUpper) && words[i + 1].Substring(2).Take(1).All(Char.IsLower))
                            result = words[i].Substring(1) + " " + words[i + 1].Substring(1);
                    }
                    catch { break; }
                    break;
                }
            }

            result = (str == null) ? str2 : result;

            if (result == string.Empty)
                result = "Неизвестно";
            A++;
            return result;
        }

        public static List<string> DB_Reader()
        {
            string pathBD = @"D:\KursRep\excel_parser\BD.txt";
            string[] mass = new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9", };
            using (StreamReader reader = new StreamReader(pathBD))
            {
                string lineDB;
                List<string> DBList = new List<string>();
                while ((lineDB = reader.ReadLine()) != "~")
                {
                    var str = mass.FirstOrDefault(x => lineDB.Contains(x));
                    if (str != null) { continue; }
                    string[] NewLine = lineDB.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
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