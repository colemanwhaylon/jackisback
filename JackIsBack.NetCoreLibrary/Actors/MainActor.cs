using System.Collections.Generic;
using System.Linq;
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
                        "/user/HashTagAnalyzerActor",
                        "/user/TopEmojisUsedAnalyzerActor",
                        "/user/TweetAverageAnalyzerActor")));


            //List<IActorRef> actorRefs = new List<IActorRef>
            //{
            //    _hashTagAnalyzerActorRef,
            //    _totalNumberOfTweetsActorRef,
            //    _tweetAverageActorRef,
            //    _topEmojisUsedActorRef,
            //    _percentOfTweetsContainingEmojisActorRef,
            //    _topHashTagsActorRef,
            //    _percentOfTweetsWithUrlActorRef,
            //    _percentOfTweetsWithPhotoUrlActorRef,
            //    _topDomainsActorRef
            //};



            //_hashTagAnalyzerActorRef = Context.ActorOf(Context.DI().Props<HashTagAnalyzerActor>());
            //_totalNumberOfTweetsActorRef = Context.ActorOf(Context.DI().Props<TotalNumberOfTweetsActor>());
            //_tweetAverageActorRef = Context.ActorOf(Context.DI().Props<TweetAverageActor>());
            //_topEmojisUsedActorRef = Context.ActorOf(Context.DI().Props<TopEmojisUsedActor>());
            //_percentOfTweetsContainingEmojisActorRef = Context.ActorOf(Context.DI().Props<PercentOfTweetsContainingEmojisActor>());
            //_topHashTagsActorRef = Context.ActorOf(Context.DI().Props<TopHashTagsActor>());
            //_percentOfTweetsWithUrlActorRef = Context.ActorOf(Context.DI().Props<PercentOfTweetsWithUrlActor>());
            //_percentOfTweetsWithPhotoUrlActorRef = Context.ActorOf(Context.DI().Props<PercentOfTweetsWithPhotoUrlActor>());
            //_topDomainsActorRef = Context.ActorOf(Context.DI().Props<TopDomainsActor>());

            Receive<string>(HandleTweet);
        }

        private void HandleTweet(string message)
        {
            _logger.Debug($"MainActor received this tweet message now: {message}");

            _routerForAllAnalyzers.Tell(message);
            _routerForAllAnalyzers.Tell(PoisonPill.Instance);
            //_hashTagAnalyzerActorRef.Tell(message);
        }
    }
}