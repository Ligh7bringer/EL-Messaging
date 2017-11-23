using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM.Model
{
    //abstract class used to implement state pattern for the different message types
    public abstract class MessageState
    {
        //properties shared by the different states
        private Message message;
        private string id;
        private string type;
        private string sender;
        private string messageText;

        //getters and setters
        public string Id { get => id; set => id = value; }
        public string Type { get => type; set => type = value; }
        internal Message Message { get => message; set => message = value; }
        public string Sender { get => sender; set => sender = value; }
        public string MessageText { get => messageText; set => messageText = value; }

        //abstract method, overriden in the child states
        public abstract void ProcessMessage();
    }
}
