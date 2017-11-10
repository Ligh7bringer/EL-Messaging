using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM.Model
{
    static class JSONHelper
    {
        private static string path = System.Environment.CurrentDirectory + @"\JSON\"; 

        public static void WriteSMS(SMSState sms)
        {
            JObject data = new JObject(
                new JProperty("Type", sms.Type),
                new JProperty("ID", sms.Id),
                new JProperty("Sender", sms.Sender),
                new JProperty("Message", sms.MessageText));

            string filename = path + sms.Id + ".json";

            File.WriteAllText(filename, data.ToString());
        }

        public static void WriteTweet(TweetState tweet)
        {
            JObject data = new JObject(
                new JProperty("Type", tweet.Type),
                new JProperty("ID", tweet.Id),
                new JProperty("Sender", tweet.Sender),
                new JProperty("Message", tweet.MessageText));

            string filename = path + tweet.Id + ".json";

            File.WriteAllText(filename, data.ToString());
        }

        public static void WriteEmail(EmailState email)
        {
            JObject data;
            if (email.Type.Equals("Standard Email Message"))
            {
                data = new JObject(
                    new JProperty("Type", email.Type),
                    new JProperty("ID", email.Id),
                    new JProperty("Sender", email.Sender),
                    new JProperty("Subject", email.Subject),
                    new JProperty("Email Text", email.MessageText));
            }
            else
            {
                data = new JObject(
                   new JProperty("Type", email.Type),
                   new JProperty("ID", email.Id),
                   new JProperty("Sender", email.Sender),
                   new JProperty("Subject", email.Subject),
                   new JProperty("Sport Centre Code", email.CentreCode),
                   new JProperty("Incident", email.Incident),
                   new JProperty("Email Text", email.MessageText));
            }

            string filename = path + email.Id + ".json";

            File.WriteAllText(filename, data.ToString());
        }
    }
}
