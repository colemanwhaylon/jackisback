using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class PercentOfTweetsContainingEmojisAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public PercentOfTweetsContainingEmojisAnalyzerActor()
        {
            _logger.Debug("PercentOfTweetsContainingEmojisAnalyzerActor created.");

            Receive<IMyTweetDTO>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(IMyTweetDTO message)
        {
            _logger.Debug($"PercentOfTweetsContainingEmojisAnalyzerActor is analyzing tweet message: {message.Tweet}");

            




            Context.ActorSelection(SharedStrings.PercentOfTweetsContainingEmojisActorPath).Tell(message);
            //Context.Self.Tell(PoisonPill.Instance);
        }
    }
}