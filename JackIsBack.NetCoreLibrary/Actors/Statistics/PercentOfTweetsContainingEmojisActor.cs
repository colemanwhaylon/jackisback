using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class PercentOfTweetsContainingEmojisActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private double? _percentOfTweetsContainingEmojis { get; set; } = 10.0;

        public PercentOfTweetsContainingEmojisActor()
        {
            _logger.Debug("PercentOfTweetsContainingEmojisActor created.");
            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
            Receive<GetAllStatisticsMessageResponse>(HandleGetAllStatisticsMessageResponse);
        }
        
        private void HandleTwitterMessageAsync(IMyTweetDTO message)
        {
            _percentOfTweetsContainingEmojis = message.PercentOfTweetsContainingEmojis;

            _logger.Debug($"Private state was updated: PercentOfTweetsContainingEmojis = {message.PercentOfTweetsContainingEmojis} ");

            var result = new GetTotalNumberOfTweetsMessage(percentOfTweetsContainingEmojis: message.PercentOfTweetsContainingEmojis);
            Context.Sender.Tell(result, Self);
        }

        private void HandleGetAllStatisticsMessageResponse(GetAllStatisticsMessageResponse message)
        {
            message.PercentOfTweetsContainingEmojis = _percentOfTweetsContainingEmojis;
            TweetStatisticsActor.IActorRefs["PercentOfTweetsWithPhotoUrlActor"].Forward(message);
        }

        
    }
}
