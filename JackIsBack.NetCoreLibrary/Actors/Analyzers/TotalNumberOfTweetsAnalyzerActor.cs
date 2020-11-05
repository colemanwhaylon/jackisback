using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class TotalNumberOfTweetsAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public TotalNumberOfTweetsAnalyzerActor()
        {
            _logger.Debug("TotalNumberOfTweetsAnalyzerActor created.");

            Receive<string>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(string text)
        {
            _logger.Debug($"TotalNumberOfTweetsAnalyzerActor is analyzing tweet message: {text}");

            Context.ActorSelection(SharedStrings.TweetStatisticsActorPath).Tell(text);
            Context.Self.Tell(PoisonPill.Instance);
        }
    }

}