using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM.Model
{
    //handles reading of external files
    static class FileParser
    {
        //data structs where processed messages and words are stored
        private static Dictionary<String, String> words;
        private static ArrayList messages = new ArrayList();

        //path to where the files are read from
        private static string path = System.Environment.CurrentDirectory;

        //getters
        public static Dictionary<string, string> Words { get => words; }
        public static ArrayList Messages { get => messages; }

        //initialises the Dictionary where textspeak is stored
        //reads the file and stores the abbreviation as Key, the full phrase as Value
        public static void Initialise()
        {
            words = new Dictionary<String, String>();
            using (var rd = new StreamReader(path + @"\textspeak\textwords.csv"))
            {
                while (!rd.EndOfStream)
                {
                    var splits = rd.ReadLine().Split(',');
                    string expanded = "";
                    if (splits.Length > 2)
                    {
                        expanded = splits[1] + ", " + splits[2];
                        expanded = expanded.Replace('"'.ToString(), "");
                    }
                    else
                        expanded = splits[1];

                    Words.Add(splits[0], expanded);
                }
            }            
        }

        //reads messages from a text file
        //any number of messages can be read and processed as long as there is at least 1 (or more) empty lines between them
        public static void ReadMessages(string fileName)
        {
            using (var rd = new StreamReader(fileName))
            {
                while (!rd.EndOfStream)
                {
                    string line = rd.ReadLine();
                    string[] lines = new string[30];

                    if (String.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    if (!String.IsNullOrWhiteSpace(line) && line.ValidateHeader())
                    {
                        lines[0] = line;
                        for(int i = 1; i < lines.Length; i++)
                        {
                            string tmp = rd.ReadLine();
                            if (String.IsNullOrWhiteSpace(tmp) || tmp.ValidateHeader())
                                break;

                            lines[i] = tmp;
                            Console.WriteLine(lines[0]);
                        }

                        if(lines[0][0].ToString().Equals("S"))
                        {
                            string id = lines[0];
                            string sender = lines[1];
                            if (!sender.ValidatePhoneNumber())
                                throw new Exception("Invalid phone number in Message with ID: " + id);
                            string text = StringHelper.GetMessageBody(lines, 3);
                            if (text.Length > 140)
                                throw new Exception("SMS cannot be longer than 140 characters!");

                            text = text.ReplaceTextSpeak();

                            SMSState sms = new SMSState(id, sender, text);
                            Console.WriteLine(sms.ToString());
                            Messages.Add(sms);                            
                        }
                        if(lines[0][0].ToString().Equals("T"))
                        {
                            string id = lines[0];
                            string sender = lines[1];
                            if (!sender.ValidateTwitterUser())
                                throw new Exception("Invalid twitter user name in Tweet with ID: " + id);
                            string text = StringHelper.GetMessageBody(lines, 3);
                            text = text.ReplaceTextSpeak();
                            if (text.Length > 140)
                                throw new Exception("SMS cannot be longer than 140 characters!");

                            text.StoreMentions();
                            text.GetHashTags();

                            TweetState tweet = new TweetState(id, sender, text);
                            Console.WriteLine(tweet.ToString());
                            Messages.Add(tweet);
                        }
                        if(lines[0][0].ToString().Equals("E"))
                        {
                            string id = lines[0];

                            string sender = lines[1];
                            if (!sender.ValidateEmailAdress())
                                throw new Exception("Invalid email address!");

                            if (!sender.ValidateEmailAdress())
                                throw new Exception("Invalid sender email address in Email with ID: " + id);

                            string subject = lines[2];
                            if (subject.Length > 20)
                                throw new Exception("Subject cannot be longer than 20 characters!");

                            if (subject.Contains("SIR"))
                            {
                                if (!subject.ValidateDate())
                                    throw new Exception("Invalid date in Email with ID: " + id);

                                string centreCode = lines[3];
                                if (!centreCode.ValidateCentreCode())
                                    throw new Exception("Invalid Centre Code in Email with ID: " + id);

                                string incident = lines[4];
                                if (!incident.ValidateIncident())
                                    throw new Exception("Invalid Nature of Incident in Email with ID: " + id);

                                string body = StringHelper.GetMessageBody(lines, 6);
                                if (body.Length > 1048)
                                    throw new Exception("Emails cannot be longer than 1048 characters!");

                                body = body.RemoveURLs(id);

                                EmailState sir = new EmailState(id, sender, subject, centreCode, incident, body);
                                StringHelper.AddToSIR(centreCode, incident);
                                Messages.Add(sir);
                                continue;
                            }

                            string text = StringHelper.GetMessageBody(lines, 4);
                            if (text.Length > 1048)
                                throw new Exception("Emails cannot be longer than 1048 characters!");

                            text = text.RemoveURLs(id);

                            EmailState email = new EmailState(id, sender, subject, text);
                            Console.WriteLine(email.ToString());
                            Messages.Add(email);
                        }

                        lines = new string[30];
                    }
                }
            }

            foreach(MessageState _m in messages)
            {
                WriteJson(_m);
            }
        }

        //calls the correct method from JSONHelper depending on the message type to write the messages in a JSON file
        private static void WriteJson(MessageState ms)
        {
            if(ms.Id[0].ToString().Equals("S"))
            {                
                JSONHelper.WriteSMS((SMSState)ms);
            }
            if (ms.Id[0].ToString().Equals("T"))
            {
                JSONHelper.WriteTweet((TweetState)ms);
            }
            if (ms.Id[0].ToString().Equals("E"))
            {
                JSONHelper.WriteEmail((EmailState)ms);
            }
        }

        //deletes everything from the messages array list
        public static void Reset()
        {
            messages = new ArrayList();
        }
    }
}
