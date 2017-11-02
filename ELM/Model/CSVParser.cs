using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM.Model
{
    static class CSVParser
    {
        private static Dictionary<String, String> words;
        private static string path = System.Environment.CurrentDirectory;

        public static Dictionary<String, String> ReadFile()
        {
            words = new Dictionary<String, String>();
            using (var rd = new StreamReader(path + "\\textspeak\\textwords.csv"))
            {
                while (!rd.EndOfStream)
                {
                    var splits = rd.ReadLine().Split(',');
                    words.Add(splits[0], splits[1]);
                    //Console.WriteLine(splits[0] + " = " + splits[1]);
                }
            }

            return words;
        }

    }
}
