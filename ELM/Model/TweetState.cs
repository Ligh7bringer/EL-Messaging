using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM.Model
{
    class TweetState : MessageState
    {
        public TweetState(MessageState state) : this(state.Message)
        {

        }

        public TweetState(Message message)
        {
            this.Message = message;
            this.Type = "Tweet";
        }

        public override void ProcessMessage()
        {
            this.Id = StringHelper.GetMessageID(Message.Header);
            this.Sender = StringHelper.Clean(Message.Body[0]);

            if (Message.Body.Length > 2)
            {
                string text = StringHelper.GetMessageBody(Message.Body, 2);
                if (text.Length < 141)
                    this.MessageText = StringHelper.ReplaceTextSpeak(text);
                else
                    throw new ArgumentOutOfRangeException("Tweet text cannot be longer than 140 characters!");
            }
            else
            {
                this.MessageText = StringHelper.ReplaceTextSpeak(Message.Body[1]);
            }

            StringHelper.CountHashTags(this.MessageText);
            StringHelper.StoreMentions(this.MessageText);

            JSONHelper.WriteTweet(this);
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
