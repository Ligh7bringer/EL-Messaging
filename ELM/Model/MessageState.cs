using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM.Model
{
    public abstract class MessageState
    {
        private Message message;
    
        internal Message Message { get => message; set => message = value; }

        public abstract void ProcessMessage();
    }
}
