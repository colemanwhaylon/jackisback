using System.Collections.Generic;
using Akka.Actor;
using Akka.DI.Core;
using Akka.Event;
using Akka.Routing;
using JackIsBack.NetCoreLibrary.Actors.Analyzers;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class MainActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        //Analyzers
        private readonly IActorRef _hashTagAnalyzerActor;
        private readonly IActorRef _totalNumberOfTweetsActorRef;
        private readonly IActorRef _tweetAverageActorRef;
        private readonly IActorRef _topEmojisUsedActorRef;
        private readonly IActorRef _percentOfTweetsContainingEmojisActorRef;
        private readonly IActorRef _topHashTagsActorRef;
        private readonly IActorRef _percentOfTweetsWithUrlActorRef;
        private readonly IActorRef _percentOfTweetsWithPhotoUrlActorRef;
        private readonly IActorRef _topDomainsActorRef;
        


        public MainActor()
        {
            _logger.Debug("MainActor created.");
            
            _hashTagAnalyzerActor = Context.ActorOf(Context.DI().Props<HashTagAnalyzerActor>());

            Receive<string>(HandleTweet);
        }

        private void HandleTweet(string message)
        {
            _logger.Debug($"MainActor received this tweet message now: {message}");
            
            _hashTagAnalyzerActor.Tell(message);
        }
    }
}