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
    //extension class for strings
    //adds methods used for validation of user input
    static class StringHelper
    {
        //data structures where hashtags, mentions, quarantined URLS, SIRs and valid incidents are stored
        private static Dictionary<string, int> hashTags = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
        private static ArrayList mentions = new ArrayList();
        private static Dictionary<string, ArrayList> Quarantined = new Dictionary<string, ArrayList>();
        private static Dictionary<string, ArrayList> SIR = new Dictionary<string, ArrayList>();
        private static ArrayList ValidIncidents = new ArrayList { "Theft of Properties", "Staff Attack", "Device Damage", "Raid",
                                    "Customer Attack", "Staff Abuse", "Bomb Threat", "Terrorism", "Suspicious Incident",
                                     "Sport Injury", "Personal Info Leak"};

        //getters
        public static Dictionary<string, int> HashTags { get => hashTags; }
        public static ArrayList Mentions { get => mentions; }

        //removes \r\n from the end of a string (added when Enter is pressed to move to a new line in a text box)
        public static string Clean(this string text)
        {
            return text.Replace("\r\n", "");
        }        

        //expands abbreviations in message text
        public static string ReplaceTextSpeak(this string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                foreach (string str in text.Split(" ".ToCharArray()))
                {
                    if (FileParser.Words.ContainsKey(str))
                    {
                        Console.WriteLine("----------ABBREVIATION: " + str);
                        text = text.Replace(str, str + " <" + FileParser.Words[str] + ">");
                        continue;
                    }
                }

            }

            return text;
        }

        //puts the whole message body in one string (as opposed to it being in an array of lines)
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

        //counts and stores hashtags in a dictionary
        public static void GetHashTags(this string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                var regex = new Regex(@"(?<=#)\w+");
                var matches = regex.Matches(text);

                foreach (Match m in matches)
                {
                    if (HashTags.ContainsKey(m.Value))
                    {
                        HashTags.TryGetValue(m.Value, out int val);
                        HashTags[m.Value] = val + 1;
                    }
                    else
                    {
                        HashTags.Add(m.Value, 1);
                    }
                }
            }

            foreach (KeyValuePair<string, int> entry in HashTags)
            {
                Console.WriteLine("key: " + entry.Key + " value: " + entry.Value);
            }
        }

        //stores mentions in an array list
        public static void StoreMentions(this string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                var regex = new Regex(@"(?<=@)\w+");
                var matches = regex.Matches(text);

                foreach (Match m in matches)
                {
                    Mentions.Add(m.Value);
                    Console.WriteLine(m.Value);
                }
            }
        }

        //finds, removes and stores URLs in an external file
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

        //returns true if center code is in the correct format
        //return false if it is not
        public static Boolean ValidateCentreCode(this string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                return Regex.IsMatch(text, @"^\d(\d|(?<!-)-)*\d$|^\d$");
            }

            return false;
        }

        //returns true if phone number is in the correct format
        //return false if it is not
        public static Boolean ValidatePhoneNumber(this string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                return Regex.IsMatch(text, @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$");
            }

            return false;
        }

        //returns true if email address is in the correct format
        //return false if it is not
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

        //returns true if twitter username is in the correct format
        //return false if it is not
        public static bool ValidateTwitterUser(this string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                return Regex.IsMatch(text, @"(?<=@)\w+");
            }

            return false;
        }

        //returns true if nature of incident is in the correct format
        //return false if it is not
        public static bool ValidateIncident(this string text)
        {
            if(!String.IsNullOrWhiteSpace(text))
                return ValidIncidents.Contains(text);

            return false;
        }

        //returns true if date is in the correct format
        //return false if it is not
        public static bool ValidateDate(this string text)
        {
            var regex = new Regex(@"(\d+)[-.\/](\d+)[-.\/](\d+)");

            return regex.IsMatch(text);
        }

        //writes an SIR ID and nature of incident to an external file
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

        //returns true if header is in the correct format
        //return false if it is not
        public static bool ValidateHeader(this string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                return false;

            return (text.Length == 10 && (text[0].ToString().Equals("T") || text[0].ToString().Equals("S") || text[0].ToString().Equals("E"))) ;
        }

        //returns true if header contains exactly 9 numbers
        //return false if it doesn't
        public static bool ValidateNumbers(this string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                return false;

            if (text.Length < 10)
                return false;

            var regex = new Regex("^\\d{9}$");        
            text = text.Substring(1, 9);

            Console.WriteLine("------------HEADER: " + text);
            return regex.IsMatch(text);
        }
    }
}
