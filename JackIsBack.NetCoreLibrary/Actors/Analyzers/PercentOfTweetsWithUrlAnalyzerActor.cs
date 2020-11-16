using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class PercentOfTweetsWithUrlAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public PercentOfTweetsWithUrlAnalyzerActor()
        {
            _logger.Debug("PercentOfTweetsWithUrlAnalyzerActor created.");

            Receive<MyTweetDTO>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(MyTweetDTO message)
        {
            _logger.Debug($"PercentOfTweetsWithUrlAnalyzerActor  is analyzing tweet message: {message.Tweet}");

            Context.ActorSelection(SharedStrings.PercentOfTweetsWithUrlActorPath).Tell(message);
            // Context.Self.Tell(PoisonPill.Instance);
        }
    }
}