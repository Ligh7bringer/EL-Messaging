using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM.Model
{
    //defines a message
    class Message
    {
        //properties of the class
        private string header;
        private string[] body;
        private MessageState currentState = null;

        //constructor which accepts the ID and the message text of the message
        public Message(string header, string[] body)
        {          
            this.header = header;
            this.body = body;
            SetState();
        }

        //sets the state of the message depending on the first letter of the header
        //throws an exception when the header doesnt contain 9 numbers
        //throws an exception when the header doesnt start with S, T or E
        public void SetState()
        {
            Console.WriteLine(header[0]);
            if(!header.ValidateNumbers())
            {
                throw new Exception("Header must be in the format S/T/E123456789!");
            }
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

        //getter and setter for header
        //throws an exception when header is null
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

        //getter and setter for body
        //throws an exception when body is null
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
        
        //getter and setter for the state of the message
        public MessageState CurrentState { get => currentState; set => currentState = value;  }
    }
}
