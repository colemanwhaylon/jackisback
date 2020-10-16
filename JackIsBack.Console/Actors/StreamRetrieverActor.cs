using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using JackIsBack.Console.Messages;

namespace JackIsBack.Console
{
    public class StreamRetrieverActor : ReceiveActor
    {
        public StreamRetrieverActor()
        {
            System.Console.WriteLine("Creating a StreamRetrieverActor");
            Receive<TwitterMessage>(message => HandleTwitterMessage(message));
        }

        private void HandleTwitterMessage(TwitterMessage message)
        {
            System.Console.WriteLine(message.Tweet);
        }

    }
}
