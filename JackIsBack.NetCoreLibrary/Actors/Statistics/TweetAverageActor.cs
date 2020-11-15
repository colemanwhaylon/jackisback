using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Messages;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class TweetAverageActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private int? _averageTweetsPerHour = 50;
        private int? _averageTweetsPerMinute = 60;
        private int? _avgTweetsPerSecond = 70;

        public TweetAverageActor()
        {
            _logger.Debug("TweetAverageActor created.");

            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
            Receive<GetAllStatisticsMessageResponse>(HandleGetAllStatisticsMessageResponse);
        }
        private void HandleTwitterMessageAsync(MyTweetDTO message)
        {
            _logger.Debug($"TweetAverageActor got message: {message} ");

        }

        private void HandleGetAllStatisticsMessageResponse(GetAllStatisticsMessageResponse message)
        {
            message.AverageTweetsPerHour = _averageTweetsPerHour;
            message.AverageTweetsPerMinute = _averageTweetsPerMinute;
            message.AverageTweetsPerSecond = _avgTweetsPerSecond;

            Sender.Tell(message, Self);
        }
    }
}
