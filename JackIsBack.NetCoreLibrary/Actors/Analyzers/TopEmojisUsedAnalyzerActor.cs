using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class TopEmojisUsedAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public TopEmojisUsedAnalyzerActor()
        {
            _logger.Debug("TopEmojisUsedAnalyzerActor created.");

            Receive<IMyTweetDTO>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(IMyTweetDTO message)
        {
            _logger.Debug($"TopEmojisUsedAnalyzerActor is analyzing tweet message: {message}");

            Context.ActorSelection(SharedStrings.TweetStatisticsActorPath).Tell(message);
           // Context.Self.Tell(PoisonPill.Instance);
        }
    }
}