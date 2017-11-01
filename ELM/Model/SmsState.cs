using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ELM.Model
{
    class SmsState : MessageState
    {
        private string Type = "SMS";
        private string id;
        private string sender;
        private string body;

        public string Id { get => id; set => id = value; }
        public string Sender { get => sender; set => sender = value; }
        public string Body { get => body; set => body = value; }


        public SmsState(MessageState state) : this(state.Message)
        {

        }

        public SmsState(Message message)
        {
            this.Message = message;
        }

        public override void ProcessMessage()
        {
            string id = Message.Header.Substring(1, 9);
            string sender = Message.Body.Substring(0, 13);
            string message = Message.Body.Substring(13, Message.Body.Length-13);

            this.Id = id;
            this.Sender = sender;
            this.Body = message;

            JObject data = new JObject(
                new JProperty("Type", this.Type),
                new JProperty("ID", this.Id),
                new JProperty("Sender", this.Sender),
                new JProperty("Message", this.Body));

            string path = System.Environment.CurrentDirectory + "\\JSON\\" + this.id + ".json";
            
            File.WriteAllText(path , data.ToString());


        }


    }
}
