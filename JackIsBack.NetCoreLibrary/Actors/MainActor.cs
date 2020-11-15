using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class MainActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        //private readonly IActorRef _routerForAllAnalyzers;

        public MainActor()
        {
            _logger.Debug("MainActor created.");

            //_routerForAllAnalyzers = Context.ActorOf(
            //    Props.Empty.WithRouter(
            //        new BroadcastGroup(
            //            SharedStrings.PercentOfTweetsContainingEmojisAnalyzerActorPath,
            //            SharedStrings.PercentOfTweetsWithPhotoUrlAnalyzerActorPath,
            //            SharedStrings.TopDomainsAnalyzerActorPath,
            //            SharedStrings.TopEmojisUsedAnalyzerActorPath,
            //            SharedStrings.TopHashTagsAnalyzerActorPath,
            //            SharedStrings.TweetAverageAnalyzerActorPath)));

            //Receive<IMyTweetDTO>(HandleTweet);
        }

        private void HandleTweet(IMyTweetDTO message)
        {
            //_logger.Debug($"MainActor got: {message.Tweet}");

            //var result = (MyTweetDTO)Context.ActorSelection(SharedStrings.TotalNumberOfTweetsActorPath).Ask(message).Result;

            //var newMessage = new GetTotalNumberOfTweetsMessage(result.CurrentTweetCount, message.PercentOfTweetsContainingEmojis,
            //    message.PercentOfTweetsWithPhotoUrl, 
            //    message.PercentOfTweetsWithUrl, 
            //    message.AverageTweetsPerHour, 
            //    message.AverageTweetsPerMinute, 
            //    message.AverageTweetsPerSecond);

            //Context.System.EventStream.Publish(result);

            //_routerForAllAnalyzers.Forward(newMessage);
        }
    }
}