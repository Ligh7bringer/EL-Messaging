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
        }

        public override void ProcessMessage()
        {
            throw new NotImplementedException();
        }
    }
}
