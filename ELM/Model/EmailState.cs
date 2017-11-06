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
        private string centreCode;
        private string incident;

        public EmailState(MessageState state) : this(state.Message)
        {

        }

        public EmailState(Message message)
        {
            this.Message = message;
            this.Type = "Standard Email Message";
        }

        public string Subject { get => subject; set => subject = value; }
        public string CentreCode { get => centreCode; set => centreCode = value; }

        public override void ProcessMessage()
        {
            this.Id = StringHelper.GetMessageID(Message.Header);
            this.Sender = StringHelper.Clean(Message.Body[0]);

            //subject
            if (Message.Body[1].Length > 20)
                throw new ArgumentOutOfRangeException("Subject cannot be more than 20 characters");
            else
                this.Subject = StringHelper.Clean(Message.Body[1]);

            //
            if(this.Subject.Contains("SIR")) {
                this.Type = "Email - Significant Incident Report";
                this.CentreCode = Message.Body[2];
                Console.WriteLine(this.CentreCode);
                if (StringHelper.ValidateCentreCode(this.CentreCode))
                    Console.WriteLine("CENTRE CODE VALID!!!");
            }

            //message text
            if (Message.Body.Length > 3)
            {
                string text = StringHelper.GetMessageBody(Message.Body, 3);

                if (text.Length < 1049)
                    this.MessageText = text;
                else
                    throw new ArgumentOutOfRangeException("Email text cannot be longer than 1048 characters!");
            }
            else
            {
                this.MessageText = Message.Body[2];
            }

            this.MessageText = StringHelper.RemoveURLs(this.MessageText);

            JSONHelper.WriteEmail(this);           
        }

        public override string ToString()
        {
            return "Type: " + this.Type +
                "\nID: " + this.Id +
                "\nSender: " + this.Sender +
                "\nSubject: " + this.Subject + 
                "\nMessage text: " + this.MessageText;
        }
    }
}
