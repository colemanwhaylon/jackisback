using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class TopDomainsAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public TopDomainsAnalyzerActor()
        {
            _logger.Debug("TopDomainsAnalyzerActor created.");

            Receive<string>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(string text)
        {
            _logger.Debug($"TopDomainsAnalyzerActor is analyzing tweet message: {text}");

            Context.ActorSelection(SharedStrings.TweetStatisticsActorPath).Tell(text);
            Context.Self.Tell(PoisonPill.Instance);
        }
    }
}