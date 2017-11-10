using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ELM.Model
{
    static class StringHelper
    {
        private static Dictionary<string, int> hashTags = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
        private static ArrayList mentions = new ArrayList();
        private static Dictionary<string, ArrayList> Quarantined = new Dictionary<string, ArrayList>();
        private static Dictionary<string, ArrayList> SIR = new Dictionary<string, ArrayList>();
        private static ArrayList ValidIncidents = new ArrayList { "Theft of Properties", "Staff Attack", "Device Damage", "Raid",
                                    "Customer Attack", "Staff Abuse", "Bomb Threat", "Terrorism", "Suspicious Incident",
                                     "Sport Injury", "Personal Info Leak"};


        public static Dictionary<String, int> GetHashTags()
        {
            return hashTags;
        }

        public static ArrayList GetMentions()
        {
            return mentions;
        }

        public static string GetMessageID(this string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
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
                foreach (KeyValuePair<string, string> entry in FileParser.Words)
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

        public static string GetMessageBody(this string[] text, int startAt)
        {
            string edited;
            if (text.Length > 3)
            {
                edited = Clean(text[startAt - 1]);
                for (int i = startAt; i < text.Length; i++)
                {
                    if (!String.IsNullOrWhiteSpace(text[i]))
                    {
                        edited += " " + StringHelper.Clean(text[i]);
                    }
                    else
                    {
                        continue;
                    }
                }

                return edited;
            }

            return String.Empty;
        }

        public static void GetHashTags(this string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                var regex = new Regex(@"(?<=#)\w+");
                var matches = regex.Matches(text);

                foreach (Match m in matches)
                {
                    if (hashTags.ContainsKey(m.Value))
                    {
                        hashTags.TryGetValue(m.Value, out int val);
                        hashTags[m.Value] = val + 1;
                    }
                    else
                    {
                        hashTags.Add(m.Value, 1);
                    }
                }
            }

            foreach (KeyValuePair<string, int> entry in hashTags)
            {
                Console.WriteLine("key: " + entry.Key + " value: " + entry.Value);
            }
        }

        public static void StoreMentions(this string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                var regex = new Regex(@"(?<=@)\w+");
                var matches = regex.Matches(text);

                foreach (Match m in matches)
                {
                    mentions.Add(m.Value);
                    Console.WriteLine(m.Value);
                }
            }
        }

        public static string RemoveURLs(this string text, string id)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                Regex regx = new Regex(@"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                                                 RegexOptions.IgnoreCase);

                var matches = regx.Matches(text);

                string path = System.Environment.CurrentDirectory + @"\lists\urls.txt";
                string write = "";
                
                foreach (Match match in matches)
                {
                    text = text.Replace(match.Value, "<URL Quarantined>");
                    if (Quarantined.ContainsKey(id))
                    {
                        Quarantined[id].Add(match.Value);
                        Console.WriteLine("------Quarantined: " + id + " " + match.Value);
                        write = id + ": " + match.Value + System.Environment.NewLine;
                    }
                    else
                    {
                        Quarantined.Add(id, new ArrayList { match.Value });
                        Console.WriteLine("------Quarantined: " + id + " " + match.Value);
                        write = id + ": " + match.Value + System.Environment.NewLine; ;
                    }

                    File.AppendAllText(path, write);
                }
            }

            return text;
        }

        public static Boolean ValidateCentreCode(this string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                return Regex.IsMatch(text, @"^\d(\d|(?<!-)-)*\d$|^\d$");
            }

            return false;
        }

        public static Boolean ValidatePhoneNumber(this string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                return Regex.IsMatch(text, @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$");
            }

            return false;
        }

        public static bool ValidateEmailAdress(this string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool ValidateTwitterUser(this string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                return Regex.IsMatch(text, @"(?<=@)\w+");
            }

            return false;
        }

        public static bool ValidateIncident(this string text)
        {
            if(!String.IsNullOrWhiteSpace(text))
                return ValidIncidents.Contains(text);

            return false;
        }

        public static bool ValidateDate(this string text)
        {
            var regex = new Regex(@"(\d+)[-.\/](\d+)[-.\/](\d+)");

            return regex.IsMatch(text);
        }
    
        public static void AddToSIR(string centreCode, string incident) 
        {
            if(!String.IsNullOrWhiteSpace(centreCode) && !String.IsNullOrWhiteSpace(incident))
            {
                if(SIR.ContainsKey(centreCode))
                {
                    SIR[centreCode].Add(incident);
                    Console.WriteLine("------SAVING: " + centreCode + " " + incident);
                }
                else
                {
                    SIR.Add(centreCode, new ArrayList { incident });
                    Console.WriteLine("------SAVING: " + centreCode + " " + incident);
                }
            }

            string path = System.Environment.CurrentDirectory + @"\lists\SIRs.txt";
            string text = centreCode + ": " + incident + Environment.NewLine;
            Console.WriteLine(text);
            File.AppendAllText(path, text);
        }

        public static bool ValidateHeader(this string text)
        {
            return (text.Length == 10 && (text[0].ToString().Equals("T") || text[0].ToString().Equals("S") || text[0].ToString().Equals("E"))) ;
        }
    }
}
