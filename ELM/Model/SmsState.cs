using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM.Model
{
    class SmsState : MessageState
    {
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
            Console.WriteLine("SMS, id: " + id);
            string sender = Message.Body.Substring(0, 13);
            Console.WriteLine("sender: " + sender);
            string message = Message.Body.Substring(13, Message.Body.Length-14);
            Console.WriteLine("message: " + message);

        }


    }
}
