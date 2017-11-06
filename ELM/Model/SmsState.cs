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
    class SMSState : MessageState
    {
        public SMSState(MessageState state) : this(state.Message)
        {

        }

        public SMSState(Message message)
        {
            this.Message = message;
            this.Type = "SMS";
        }

        public override void ProcessMessage()
        {
            this.Id = Message.Header.GetMessageID();
            this.Sender = Message.Body[0].Clean();

            string text = StringHelper.GetMessageBody(Message.Body, 2);

            if (text.Length < 141)
                this.MessageText = StringHelper.ReplaceTextSpeak(text);
            else
                throw new ArgumentOutOfRangeException("SMS text cannot be longer than 140 characters!");
           
            JSONHelper.WriteSMS(this);
        }

        public override string ToString()
        {
            return "Type: " + this.Type +
                "\nID: " + this.Id +
                "\nSender: " + this.Sender +
                "\nMessage text: " + this.MessageText;
        }


    }
}
