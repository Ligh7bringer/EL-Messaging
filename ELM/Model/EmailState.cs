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
        public string Incident { get => incident; set => incident = value; }

        public override void ProcessMessage()
        {
            this.Id = StringHelper.GetMessageID(Message.Header);
            this.Sender = StringHelper.Clean(Message.Body[0]);

            //subject
            if (Message.Body[1].Length > 20)
                throw new ArgumentOutOfRangeException("Subject cannot be more than 20 characters");
            else
                this.Subject = StringHelper.Clean(Message.Body[1]);

            //handle SIRs
            if (this.Subject.Contains("SIR"))
            {
                this.Type = "Email - Significant Incident Report";
                this.CentreCode = Message.Body[2].Clean();

                if (!CentreCode.ValidateCentreCode())
                    throw new Exception("Invalid centre code!");

                this.Incident = Message.Body[3].Clean();
                                
                this.MessageText = StringHelper.GetMessageBody(Message.Body, 4);
                
            }
            else //handle regular emails
            {
                //message text
                this.MessageText = Message.Body.GetMessageBody(3);             
            }

            if (this.MessageText.Length > 1049)
                throw new Exception("Email text cannot be longer than 1048 characters!");

            this.MessageText = MessageText.RemoveURLs();
        
            JSONHelper.WriteEmail(this);           
        }

        public override string ToString()
        {
            if (this.Type.Equals("Standard Email Message"))
            {
                return "Type: " + this.Type +
                        "\nID: " + this.Id +
                        "\nSender: " + this.Sender +
                        "\nSubject: " + this.Subject +
                        "\nMessage text: " + this.MessageText;
            }
            else
            {
                return "Type: " + this.Type +
                        "\nID: " + this.Id +
                        "\nSender: " + this.Sender +
                        "\nSubject: " + this.Subject +
                        "\nCentre code: " + this.CentreCode +
                        "\nIncident: " + this.Incident +
                        "\nMessage text: " + this.MessageText;
            }
        }
    }
}
