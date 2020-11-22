using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Messages;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class PercentOfTweetsContainingEmojisActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private double? _percentOfTweetsContainingEmojis = 0.0;
        private int? _tweetCount = 0;

        public PercentOfTweetsContainingEmojisActor()
        {
            _logger.Debug("PercentOfTweetsContainingEmojisActor created.");
            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
            Receive<GetAllStatisticsMessageResponse>(HandleGetAllStatisticsMessageResponse);
        }

        private void HandleTwitterMessageAsync(MyTweetDTO message)
        {
            _tweetCount = message.CurrentTweetCount;
            _percentOfTweetsContainingEmojis = message.PercentOfTweetsContainingEmojis;
        }

        private void HandleGetAllStatisticsMessageResponse(GetAllStatisticsMessageResponse message)
        {
            message.TotalNumberOfTweets = _tweetCount;
            message.PercentOfTweetsContainingEmojis = _percentOfTweetsContainingEmojis;
            TweetStatisticsActor.IActorRefs["PercentOfTweetsWithPhotoUrlActor"].Forward(message);
        }


    }
}
