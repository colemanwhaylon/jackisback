using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.DI.Core;
using Akka.Event;
using Akka.Routing;
using JackIsBack.NetCoreLibrary.Actors.Analyzers;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class MainActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        //Analyzers
        private readonly IActorRef _routerForAllAnalyzers;

        private readonly IActorRef _hashTagAnalyzerActorRef;
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

            _routerForAllAnalyzers = Context.ActorOf(
                Props.Empty.WithRouter(
                    new BroadcastGroup(
                        SharedStrings.PercentOfTweetsContainingEmojisAnalyzerActorPath,
                        SharedStrings.PercentOfTweetsWithPhotoUrlAnalyzerActorPath,
                        SharedStrings.TopDomainsAnalyzerActorPath,
                        SharedStrings.TopEmojisUsedAnalyzerActorPath,
                        SharedStrings.TopHashTagsAnalyzerActorPath,
                        SharedStrings.TotalNumberOfTweetsAnalyzerActorPath,
                        SharedStrings.TweetAverageAnalyzerActorPath)));

            Receive<string>(HandleTweet);
        }

        private void HandleTweet(string message)
        {
            _logger.Debug($"MainActor received this tweet message now: {message}");

            _routerForAllAnalyzers.Tell(message);
            
            Context.Self.Tell(PoisonPill.Instance);
        }
    }
}