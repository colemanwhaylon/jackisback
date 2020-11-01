using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Commands;
using System.Collections.Generic;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class HashTagAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public HashTagAnalyzerActor()
        {
            _logger.Debug("HashTagAnalyzerActor created.");
            Receive<string>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(string text)
        {



            //Context.ActorSelection("akka://TwitterStatisticsActorSystem/user/TweetStatisticsActor").Tell(command);
        }
    }
}
