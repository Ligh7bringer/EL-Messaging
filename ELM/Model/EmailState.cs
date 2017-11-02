using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM.Model
{
    class EmailState : MessageState
    {
        private string subject;

        public EmailState(MessageState state) : this(state.Message)
        {

        }

        public EmailState(Message message)
        {
            this.Message = message;
            this.Type = "Email";
        }

        public string Subject { get => subject; set => subject = value; }

        public override void ProcessMessage()
        {
            this.Id = StringHelper.GetMessageID(Message.Header);
            this.Sender = StringHelper.Clean(Message.Body[0]);

            if (Message.Body[1].Length > 20)
                throw new ArgumentOutOfRangeException("Subject cannot be more than 20 characters");
            else
                this.Subject = StringHelper.Clean(Message.Body[1]);

            if (Message.Body[2].Length > 1048)
                throw new ArgumentOutOfRangeException("Email text cannot be longer than 1048 characters.");
            else
                this.MessageText = Message.Body[2];

            JSONHelper.WriteEmail(this);
            
        }
    }
}
