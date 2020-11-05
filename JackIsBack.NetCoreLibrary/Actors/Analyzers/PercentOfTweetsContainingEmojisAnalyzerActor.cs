using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class PercentOfTweetsContainingEmojisAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public PercentOfTweetsContainingEmojisAnalyzerActor()
        {
            _logger.Debug("PercentOfTweetsContainingEmojisAnalyzerActor created.");
            
            Receive<string>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(string text)
        {
            _logger.Debug($"PercentOfTweetsContainingEmojisAnalyzerActor is analyzing tweet message: {text}");

            Context.ActorSelection(SharedStrings.TweetStatisticsActorPath).Tell(text);
            Context.Self.Tell(PoisonPill.Instance);
        }
    }
}