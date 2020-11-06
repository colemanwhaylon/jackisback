using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class TopDomainsAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public TopDomainsAnalyzerActor()
        {
            _logger.Debug("TopDomainsAnalyzerActor created.");

            Receive<IMyTweetDTO>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(IMyTweetDTO message)
        {
            _logger.Debug($"TopDomainsAnalyzerActor is analyzing tweet message: {message.Tweet}");

            Context.ActorSelection(SharedStrings.TopDomainsActorPath).Tell(message);
            //Context.Self.Tell(PoisonPill.Instance);
        }
    }
}