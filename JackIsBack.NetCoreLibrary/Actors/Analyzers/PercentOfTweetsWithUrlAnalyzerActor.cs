using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class PercentOfTweetsWithUrlAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public PercentOfTweetsWithUrlAnalyzerActor()
        {
            _logger.Debug("PercentOfTweetsWithUrlAnalyzerActor created.");

            Receive<IMyTweetDTO>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(IMyTweetDTO message)
        {
            _logger.Debug($"PercentOfTweetsWithUrlAnalyzerActor  is analyzing tweet message: {message.Tweet}");

            Context.ActorSelection(SharedStrings.TweetStatisticsActorPath).Tell(message);
           // Context.Self.Tell(PoisonPill.Instance);
        }
    }
}