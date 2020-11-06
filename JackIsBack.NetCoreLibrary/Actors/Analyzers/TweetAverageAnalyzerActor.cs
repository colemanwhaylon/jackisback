using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class TweetAverageAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public TweetAverageAnalyzerActor()
        {
            _logger.Debug("TweetAverageAnalyzerActor created.");

            Receive<IMyTweetDTO>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(IMyTweetDTO message)
        {
            _logger.Debug($"TweetAverageAnalyzerActor  is analyzing tweet message: {message}");

            Context.ActorSelection(SharedStrings.TweetAverageActorPath).Tell(message);
            // Context.Self.Tell(PoisonPill.Instance);
        }
    }
}