using Akka.Actor;
using Akka.DI.Core;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Actors.Analyzers;
using JackIsBack.NetCoreLibrary.Actors.Statistics;
using JackIsBack.NetCoreLibrary.Messages;
using System;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class TweetStatisticsActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private TimeSpan _startDateTime = TimeSpan.Zero;
        private TimeSpan _endDateTime = TimeSpan.Zero;

        //Initialize Analyzer Actors
        private IActorRef _topHashTagsAnalyzerActorRef;
        private IActorRef _topEmojisUsedAnalyzerActorRef;
        private IActorRef _tweetAverageAnalyzerActorRef;
        private IActorRef _topDomainsAnalyzerActorRef;
        private IActorRef _percentOfTweetsWithPhotoUrlAnalyzerActorRef;
        private IActorRef _percentOfTweetsContainingEmojisAnalyzerActorRef;
        private IActorRef _percentOfTweetsWithUrlAnalyzerActorRef;

        //Initialize Statistics Actors
        private IActorRef _topHashTagsActorRef;
        private IActorRef _topEmojisUsedActorRef;
        private IActorRef _tweetAverageActorRef;
        private IActorRef _topDomainsActorRef;
        private IActorRef _percentOfTweetsWithPhotoUrlActorRef;
        private IActorRef _percentOfTweetsContainingEmojisActorRef;
        private IActorRef _percentOfTweetsWithUrlActorRef;
        //private IActorRef _totalNumberOfTweetsActorRef;

        public TweetStatisticsActor()
        {
            _logger.Debug("TweetStatisticsActor created.");

            // Init Analyzer Actors
            //akka://TwitterStatisticsActorSystem/user/TweetStatisticsActor/PercentOfTweetsContainingEmojisAnalyzerActor
            _percentOfTweetsContainingEmojisAnalyzerActorRef = Context.ActorOf(Context.DI().Props<PercentOfTweetsContainingEmojisAnalyzerActor>(), "PercentOfTweetsContainingEmojisAnalyzerActor");
            _percentOfTweetsWithPhotoUrlAnalyzerActorRef = Context.ActorOf(Context.DI().Props<PercentOfTweetsWithPhotoUrlAnalyzerActor>(), "PercentOfTweetsWithPhotoUrlAnalyzerActor");
            _percentOfTweetsWithUrlAnalyzerActorRef = Context.ActorOf(Context.DI().Props<PercentOfTweetsWithUrlAnalyzerActor>(), "PercentOfTweetsWithUrlAnalyzerActor");
            _topDomainsAnalyzerActorRef = Context.ActorOf(Context.DI().Props<TopDomainsAnalyzerActor>(), "TopDomainsAnalyzerActor");
            _topEmojisUsedAnalyzerActorRef = Context.ActorOf(Context.DI().Props<TopEmojisUsedAnalyzerActor>(), "TopEmojisUsedAnalyzerActor");
            _topHashTagsAnalyzerActorRef = Context.ActorOf(Context.DI().Props<TopHashTagsAnalyzerActor>(), "TopHashTagsAnalyzerActor");
            _tweetAverageAnalyzerActorRef = Context.ActorOf(Context.DI().Props<TweetAverageAnalyzerActor>(), "TweetAverageAnalyzerActor");

            // Init Statistics Actors
            _percentOfTweetsContainingEmojisActorRef = Context.ActorOf(Context.DI().Props<PercentOfTweetsContainingEmojisActor>(), "PercentOfTweetsContainingEmojisActor");
            _percentOfTweetsWithPhotoUrlActorRef = Context.ActorOf(Context.DI().Props<PercentOfTweetsWithPhotoUrlActor>(), "PercentOfTweetsWithPhotoUrlActor");
            _percentOfTweetsWithUrlActorRef = Context.ActorOf(Context.DI().Props<PercentOfTweetsWithUrlActor>(), "PercentOfTweetsWithUrlActor");
            _topDomainsActorRef = Context.ActorOf(Context.DI().Props<TopDomainsActor>(), "TopDomainsActor");
            _topEmojisUsedActorRef = Context.ActorOf(Context.DI().Props<TopEmojisUsedActor>(), "TopEmojisUsedActor");
            _topHashTagsActorRef = Context.ActorOf(Context.DI().Props<TopHashTagsActor>(), "TopHashTagsActor");
            _tweetAverageActorRef = Context.ActorOf(Context.DI().Props<TweetAverageActor>(), "TweetAverageActor");

            

            // Declare messages to Receive 
            Receive<GetAllStatisticsMessageResponse>(HandleGetAllStatisticsMessage);
            Receive<TimeKeeperActorMessage>(HandleTimeKeeperActorMessage);
        }

        private void HandleGetAllStatisticsMessage(GetAllStatisticsMessageResponse messageResponse)
        {
            _logger.Debug($"TweetStatisticsActor got messageResponse: {messageResponse}.");
            var result = new GetAllStatisticsMessageResponse(3);
            _logger.Debug($"TweetStatisticsActor sending back: {result}.");
            Context.Sender.Tell(result);
        }

        private void HandleTimeKeeperActorMessage(TimeKeeperActorMessage message)
        {
            _startDateTime = message.StartDateTime;
            _endDateTime = message.EndDateTime;
        }

        public override void AroundPreStart()
        {
            base.AroundPreStart();
            _logger.Debug("AroundPreStart() executed HERE NOW!");
        }

        //todo:Declare method that can be asked for by a client to get all statistic actor's values

    }
}