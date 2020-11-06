using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class PercentOfTweetsWithPhotoUrlAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public PercentOfTweetsWithPhotoUrlAnalyzerActor()
        {
            _logger.Debug("PercentOfTweetsWithPhotoUrlAnalyzerActor created.");

            Receive<IMyTweetDTO>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(IMyTweetDTO message)
        {
            _logger.Debug($"PercentOfTweetsWithPhotoUrlAnalyzerActor is analyzing tweet message: {message.Tweet}");

            Context.ActorSelection(SharedStrings.TweetStatisticsActorPath).Tell(message);
            //Context.Self.Tell(PoisonPill.Instance);
        }
    }
}