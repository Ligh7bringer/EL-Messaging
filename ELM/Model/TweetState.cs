using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM.Model
{
    //defines a tweet message
    class TweetState : MessageState
    {
        //chained constructor, set the actual message
        public TweetState(MessageState state) : this(state.Message)
        {

        }

        public TweetState(Message message)
        {
            this.Message = message;
            this.Type = "Tweet";
        }

        //constructor used when messages are read from a file
        public TweetState(string id, string sender, string text)
        {
            this.Type = "Tweet";
            this.Id = id;
            this.Sender = sender;
            this.MessageText = text;
        }

        //overrides the method in the parent class
        public override void ProcessMessage()
        {
            //extract ID and sender
            this.Id = Message.Header;
            this.Sender = Message.Body[0].Clean();
            if (!this.Sender.ValidateTwitterUser())
                throw new Exception("Invalid twitter username!");
       
            string text = StringHelper.GetMessageBody(Message.Body, 2);
            MessageText = text; 

            MessageText.StoreMentions();
            MessageText.GetHashTags();

            if (text.Length < 141)
                this.MessageText = StringHelper.ReplaceTextSpeak(text);
            else
                throw new ArgumentOutOfRangeException("Tweet text cannot be longer than 140 characters!");            

            JSONHelper.WriteTweet(this);
        }

        //overriden ToString, used when displaying processed messages
        public override string ToString()
        {
            return "Type: " + this.Type +
                "\nID: " + this.Id +
                "\nSender: " + this.Sender +
                "\nMessage text: " + this.MessageText;
        }
    }
}
