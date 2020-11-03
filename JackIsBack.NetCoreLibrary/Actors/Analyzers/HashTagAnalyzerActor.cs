using Akka.Actor;
using Akka.Event;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class HashTagAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private IActorRef _tweetStatisticsActorRef;
        public HashTagAnalyzerActor()
        {
            _logger.Debug("HashTagAnalyzerActor created.");

            Receive<string>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(string text)
        {
            _logger.Debug($"HashTagAnalyzerActor is analyzing tweet message: {text}");

            Context.ActorSelection("/user/TweetGenerator/TweetStatisticsActor").Tell(text);
        }
    }
}
