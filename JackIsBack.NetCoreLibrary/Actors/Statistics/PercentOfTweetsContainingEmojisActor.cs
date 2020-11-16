using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Messages;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class PercentOfTweetsContainingEmojisActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private double? _percentOfTweetsContainingEmojis { get; set; } = 10.0;
        private int? _tweetCount { get; set; } = 0;

        public PercentOfTweetsContainingEmojisActor()
        {
            _logger.Debug("PercentOfTweetsContainingEmojisActor created.");
            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
            Receive<GetAllStatisticsMessageResponse>(HandleGetAllStatisticsMessageResponse);
        }

        private void HandleTwitterMessageAsync(MyTweetDTO message)
        {
            _percentOfTweetsContainingEmojis = message.PercentOfTweetsContainingEmojis;
            _tweetCount = message.CurrentTweetCount;
        }

        private void HandleGetAllStatisticsMessageResponse(GetAllStatisticsMessageResponse message)
        {
            message.TotalNumberOfTweets = _tweetCount;
            message.PercentOfTweetsContainingEmojis = _percentOfTweetsContainingEmojis;
            TweetStatisticsActor.IActorRefs["PercentOfTweetsWithPhotoUrlActor"].Forward(message);
        }


    }
}
