using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class Conversation
    {
        public User Sender = new User();
        public User Receiver = new User();
        public Sentence Sentence = new Sentence();
    }
}
