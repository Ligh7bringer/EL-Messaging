using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM.Model
{
    static class StringHelper
    {

        public static string GetMessageID(this string text)
        {
            if(!string.IsNullOrWhiteSpace(text))
            {
                return text.Substring(1, 9);
            }

            return String.Empty;
        }

        public static string GetUntilSpace(this string text, string stopAt = " ")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return String.Empty;
        }

        public static string Clean(this string text)
        {
            return text.Replace("\r\n", "");
        }


        public static string ReplaceTextSpeak(this string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                foreach (KeyValuePair<string, string> entry in CSVParser.ReadFile())
                {
                    if (text.Contains(entry.Key))
                    {
                        Console.WriteLine("Found one!");
                        text = text.Replace(entry.Key, entry.Key + " <" + entry.Value + ">");
                        continue;
                    }
                }
            }
                       
            return text;      
        }

    }
}
