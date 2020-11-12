using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;
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
            Receive<IMyTweetDTO>(HandleTwitterMessageAsync);
            Receive<RefreshStatisticsRequest>(HandleRefreshStatisticsRequest);
            Receive<GetTotalNumberOfTweetsMessage>(HandleGetTotalNumberOfTweetsMessage);
        }
        private void HandleGetTotalNumberOfTweetsMessage(GetTotalNumberOfTweetsMessage message)
        {
            _logger.Debug($"TweetAverageActor.HandleGetTotalNumberOfTweetsMessage() got message: {message} AverageTweetsPerHour is now: {_averageTweetsPerHour}, AverageTweetsPerMinute is now: {_averageTweetsPerMinute}, AverageTweetsPerSecond is now: {_avgTweetsPerSecond}");
            message.AverageTweetsPerHour = _averageTweetsPerHour;
            message.AverageTweetsPerMinute = _averageTweetsPerMinute;
            message.AverageTweetsPerSecond = _avgTweetsPerSecond;

            Sender.Tell(message, Self);
        }

        private void HandleRefreshStatisticsRequest(RefreshStatisticsRequest obj)
        {
            var response = new GetAllStatisticsMessageResponse();
            response.AverageTweetsPerHour = _averageTweetsPerHour;
            response.AverageTweetsPerMinute = _averageTweetsPerMinute;
            response.AverageTweetsPerSecond = _avgTweetsPerSecond;
            Sender.Tell(response, Self);
        }

        private void HandleTwitterMessageAsync(IMyTweetDTO message)
        {
            _logger.Debug($"TweetAverageActor got message: {message} ");

            //var interval =  new TimeSpan(DateTime.Now.Subtract(TweetStatistics.StartDateTime).Ticks);
            //var totalTweets = _amount + TweetStatistics.TotalTweetCount;
            //var avgTweetsPerHour = totalTweets / interval.TotalHours;
            //var avgTweetsPerMinute = totalTweets / interval.TotalMinutes;
            //var avgTweetsPerSecond = totalTweets / interval.TotalSeconds;

            //TweetStatistics.AverageTweetsPerHour = (long)avgTweetsPerHour;
            //TweetStatistics.AverageTweetsPerMinute = (long)avgTweetsPerMinute;
            //TweetStatistics.AverageTweetsPerSecond = (long)avgTweetsPerSecond;
        }
    }
}
