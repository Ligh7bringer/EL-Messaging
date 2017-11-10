using System;
using System.Collections.Generic;
using System.Globalization;
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
            
        }

        public EmailState(string id, string sender, string subject, string text)
        {
            this.Type = "Standard Email Message";
            this.Id = id;
            this.Sender = sender;
            this.Subject = subject;
            this.MessageText = text;
        }

        public EmailState(string id, string sender, string subject, string centreCode, string incident, string text) : this(id, sender, subject, text)
        {
            this.Type = "SIR";        
            this.CentreCode = centreCode;
            this.Incident = incident;
        }

        public string Subject { get => subject; set => subject = value; }
        public string CentreCode { get => centreCode; set => centreCode = value; }
        public string Incident { get => incident; set => incident = value; }

        public override void ProcessMessage()
        {
            this.Id = StringHelper.GetMessageID(Message.Header);
            this.Sender = StringHelper.Clean(Message.Body[0]);
            if(!this.Sender.ValidateEmailAdress())
            {
                throw new Exception("Invalid email address!");
            }

            //subject
            if (Message.Body[1].Length > 20)
                throw new Exception("Subject cannot be more than 20 characters");
            else
                this.Subject = StringHelper.Clean(Message.Body[1]);

            //handle SIRs
            if (this.Subject.Contains("SIR"))
            {
                if(!this.Subject.ValidateDate())
                {
                    throw new Exception("Invalid date!");
                }

                this.Type = "Email - Significant Incident Report";
                this.CentreCode = Message.Body[2].Clean();

                if (!CentreCode.ValidateCentreCode())
                    throw new Exception("Invalid centre code!");

                this.Incident = Message.Body[3].Clean();
                if (!Incident.ValidateIncident())
                    throw new Exception("Invalid nature of incident!");
                                
                this.MessageText = StringHelper.GetMessageBody(Message.Body, 5);

                StringHelper.AddToSIR(this.CentreCode, this.Incident);
            }
            else //handle regular emails
            {
                //message text
                this.MessageText = Message.Body.GetMessageBody(3);             
            }

            if (this.MessageText.Length > 1049)
                throw new Exception("Email text cannot be longer than 1048 characters!");

            this.MessageText = MessageText.RemoveURLs(this.Id);
        
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
