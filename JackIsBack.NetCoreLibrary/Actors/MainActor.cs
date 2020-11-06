using Akka.Actor;
using Akka.Event;
using Akka.Routing;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class MainActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly IActorRef _routerForAllAnalyzers;

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
                        SharedStrings.TweetAverageAnalyzerActorPath)));

            Receive<IMyTweetDTO>(HandleTweet);
        }

        private void HandleTweet(IMyTweetDTO message)
        {
            _logger.Debug($"MainActor received this tweet message now: {message.Tweet}");
            _routerForAllAnalyzers.Tell(message);
        }
    }
}