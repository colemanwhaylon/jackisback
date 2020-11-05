using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;
using System.Linq;
using System.Collections;
using Akka.DI.Core;
using JackIsBack.NetCoreLibrary.Actors.Analyzers;
using JackIsBack.NetCoreLibrary.Commands;
using JackIsBack.NetCoreLibrary.Messages;
using ChangeTotalNumberOfTweetsMessage = JackIsBack.NetCoreLibrary.Commands.ChangeTotalNumberOfTweetsMessage;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class TweetStatisticsActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private TimeSpan _startDateTime = TimeSpan.Zero;
        private TimeSpan _endDateTime  = TimeSpan.Zero;

        //Initialize Analyzer Actors
        private IActorRef _totalNumberOfTweetsActorRef;
        private IActorRef _topHashTagsAnalyzerActorRef;
        private IActorRef _topEmojisUsedAnalyzerActorRef;
        private IActorRef _tweetAverageAnalyzerActorRef;
        private IActorRef _topDomainsAnalyzerActorRef;
        private IActorRef _percentOfTweetsWithPhotoUrlAnalyzerActorRef;
        private IActorRef _percentOfTweetsContainingEmojisAnalyzerActorRef;
        private IActorRef _percentOfTweetsWithUrlAnalyzerActorRef;

        public TweetStatisticsActor()
        {
            _logger.Debug("TweetStatisticsActor created.");

            // Init Analyzer Actors
            _percentOfTweetsContainingEmojisAnalyzerActorRef = Context.ActorOf(Context.DI().Props<PercentOfTweetsContainingEmojisAnalyzerActor>(), "PercentOfTweetsContainingEmojisAnalyzerActor");
            _percentOfTweetsWithPhotoUrlAnalyzerActorRef = Context.ActorOf(Context.DI().Props<PercentOfTweetsWithPhotoUrlAnalyzerActor>(), "PercentOfTweetsWithPhotoUrlAnalyzerActor");
            _percentOfTweetsWithUrlAnalyzerActorRef = Context.ActorOf(Context.DI().Props<PercentOfTweetsWithUrlAnalyzerActor>(), "PercentOfTweetsWithUrlAnalyzerActor");
            _topDomainsAnalyzerActorRef = Context.ActorOf(Context.DI().Props<TopDomainsAnalyzerActor>(), "TopDomainsAnalyzerActor");
            _topEmojisUsedAnalyzerActorRef = Context.ActorOf(Context.DI().Props<TopEmojisUsedAnalyzerActor>(), "TopEmojisUsedAnalyzerActor");
            _topHashTagsAnalyzerActorRef = Context.ActorOf(Context.DI().Props<TopHashTagsAnalyzerActor>(), "TopHashTagsAnalyzerActor");
            _totalNumberOfTweetsActorRef = Context.ActorOf(Context.DI().Props<TotalNumberOfTweetsActor>(), "TotalNumberOfTweetsActor");
            _tweetAverageAnalyzerActorRef = Context.ActorOf(Context.DI().Props<TweetAverageAnalyzerActor>(), "TweetAverageAnalyzerActor");

            Receive<TimeKeeperActorMessage>(HandleTimeKeeperActorMessage);
        }

        private void HandleTimeKeeperActorMessage(TimeKeeperActorMessage message)
        {
            _startDateTime = message.StartDateTime;
            _endDateTime = message.EndDateTime;
        }

        private void HandleTweet(string message)
        {
            _logger.Debug($"TweetStatistics got tweet message: {message} from {Sender.Path}");
        }

        private void HandleUpdateHashTagsCommand(UpdateHashTagsCommand command)
        {
            command.Execute();

            _logger.Debug($"TweetStatistics.HashTags key count: {TweetStatistics.HashTags.Keys.Count}");
            TweetStatistics.HashTags.ToList().Sort((x,y)=> x.Value.CompareTo(y.Value));
            var list = TweetStatistics.HashTags.OrderByDescending((x) => x.Value).Take(5);
            _logger.Debug($"TweetStatistics.HashTags top 5: ");
            var count = 0;
            foreach (var item in list)
            {
                count++;
                _logger.Debug($"#{count}: {item}");
            }

        }

        private static int CompareLongs(KeyValuePair<string, long> a, KeyValuePair<string, long> b)
        {
            return a.Value.CompareTo(b.Value);
        }

        private void HandleTweetAverageCommand(UpdateTweetAverageCommand command)
        {
            command.Execute();

            //_logger.Debug($"Command: {command.ToString()}");
            _logger.Debug($"TweetStatistics.AverageTweetsPerHour: {TweetStatistics.AverageTweetsPerHour}\n" +
                          $"\tTweetStatistics.AverageTweetsPerMinute: {TweetStatistics.AverageTweetsPerMinute}\n" +
                          $"\tTweetStatistics.AverageTweetsPerSecond: {TweetStatistics.AverageTweetsPerSecond}");
        }

    }

}