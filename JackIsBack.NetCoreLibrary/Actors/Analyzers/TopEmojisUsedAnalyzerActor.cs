using Akka.Actor;
using Akka.Event;

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

            Context.ActorSelection("/user/TweetGeneratorActor/TweetAverageAnalyzerActor").Tell(text);
            Context.Self.Tell(PoisonPill.Instance);
        }
    }
}