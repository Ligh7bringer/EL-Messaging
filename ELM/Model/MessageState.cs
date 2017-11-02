using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM.Model
{
    public abstract class MessageState
    {
        private Message message;
        private string id;
        private string type;
        private string sender;
        private string messageText;

        public string Id { get => id; set => id = value; }
        public string Type { get => type; set => type = value; }
        internal Message Message { get => message; set => message = value; }
        public string Sender { get => sender; set => sender = value; }
        public string MessageText { get => messageText; set => messageText = value; }

        public abstract void ProcessMessage();
    }
}
