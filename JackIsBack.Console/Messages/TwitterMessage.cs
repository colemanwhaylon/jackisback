using System;
using System.Collections.Generic;
using System.Text;
using JackIsBack;

namespace JackIsBack.Console.Messages
{
    public class TwitterMessage
    {
        public Root Tweet { get; private set; }

        public TwitterMessage(Root tweet)
        {
            Tweet = tweet;
        }
    }
}
