using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class TopEmojisUsedAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public TopEmojisUsedAnalyzerActor()
        {
            _logger.Debug("TopEmojisUsedAnalyzerActor created.");

            Receive<MyTweetDTO>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(MyTweetDTO message)
        {
            _logger.Debug($"TopEmojisUsedAnalyzerActor is analyzing tweet message: {message}");

            Context.ActorSelection(SharedStrings.TopEmojisUsedActorPath).Tell(message);
            // Context.Self.Tell(PoisonPill.Instance);
        }
    }
}