using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM.Model
{
    class Message
    {
        //private Type messageType;
        private string header;
        private string[] body;
        private MessageState currentState = null;

        public Message(string header, string[] body)
        {          
            this.header = header;
            this.body = body;
            SetState();
        }

        public void SetState()
        {
            Console.WriteLine(header[0]);
            if (header[0].ToString().Equals("S"))
            {
                currentState = new SMSState(this);
            }
            else if (header[0].ToString().Equals("E"))
            {
                this.currentState = new EmailState(this);
            }
            else if (header[0].ToString().Equals("T"))
            {
                this.currentState = new TweetState(this);
            }
            else
            {
                throw new Exception("Invalid header.");
            }

            currentState.ProcessMessage();
        }

        public string Header
        {
            get => header;
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    header = value;
                } else
                {
                    throw new Exception("A message has to have a header!");
                }
            }
        }

        public string[] Body { get => body;
            set
            {
                if (!String.IsNullOrEmpty(value[0]))
                {
                    body = value;
                }
                else
                {
                    throw new Exception("A message has to have a body!");
                }
            }
        }
        
        public MessageState CurrentState { get => currentState; set => currentState = value;  }
    }
}
