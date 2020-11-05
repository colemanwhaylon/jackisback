using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class TopEmojisUsedAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public TopEmojisUsedAnalyzerActor()
        {
            _logger.Debug("TopEmojisUsedAnalyzerActor created.");

            Receive<string>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(string text)
        {
            _logger.Debug($"TopEmojisUsedAnalyzerActor is analyzing tweet message: {text}");

            Context.ActorSelection(SharedStrings.TweetStatisticsActorPath).Tell(text);
            Context.Self.Tell(PoisonPill.Instance);
        }
    }
}