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
            this.MessageText = StringHelper.ReplaceTextSpeak(Message.Body[1]);



            JSONHelper.WriteTweet(this);
        }
    }
}
