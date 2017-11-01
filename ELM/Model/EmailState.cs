using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM.Model
{
    class EmailState : MessageState
    {
        public EmailState(MessageState state) : this(state.Message)
        {

        }

        public EmailState(Message message)
        {
            this.Message = message;
        }

        public override void ProcessMessage()
        {
            throw new NotImplementedException();
        }
    }
}
