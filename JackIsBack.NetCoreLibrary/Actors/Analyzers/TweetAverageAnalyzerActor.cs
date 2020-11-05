using Akka.Actor;
using Akka.Event;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class TweetAverageAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public TweetAverageAnalyzerActor()
        {
            _logger.Debug("TweetAverageAnalyzerActor created.");

            Receive<string>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(string text)
        {
            _logger.Debug($"TweetAverageAnalyzerActor  is analyzing tweet message: {text}");

            Context.ActorSelection("/user/TweetGeneratorActor/TweetAverageAnalyzerActor").Tell(text);
            Context.Self.Tell(PoisonPill.Instance);
        }
    }
}