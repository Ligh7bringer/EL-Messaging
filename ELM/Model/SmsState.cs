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
            this.Id = StringHelper.GetMessageID(Message.Header);
            this.Sender = StringHelper.Clean(Message.Body[0]);
            this.MessageText = StringHelper.ReplaceTextSpeak(Message.Body[1]);

            if (this.MessageText.Length > 140)
            {
                throw new ArgumentOutOfRangeException("Sms text cannot be longer than 140 characters!");
            }

            JSONHelper.WriteSMS(this);
        }


    }
}
