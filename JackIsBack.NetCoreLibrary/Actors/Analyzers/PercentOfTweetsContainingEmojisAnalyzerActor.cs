using System;
using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class PercentOfTweetsContainingEmojisAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();

        public PercentOfTweetsContainingEmojisAnalyzerActor()
        {
            _logger.Debug("PercentOfTweetsContainingEmojisAnalyzerActor created.");

            Receive<MyTweetDTO>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(MyTweetDTO message)
        {
            _logger.Debug($"PercentOfTweetsContainingEmojisAnalyzerActor is analyzing tweet message: {message.Tweet}");

            var actor = TweetStatisticsActor.IActorRefs["PercentOfTweetsContainingEmojisActor"];
            actor.Tell(message);

            //Context.Self.Tell(PoisonPill.Instance);
        }
    }
}