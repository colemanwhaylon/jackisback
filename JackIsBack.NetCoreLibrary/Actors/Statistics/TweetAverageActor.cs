using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class TweetAverageActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();

        public TweetAverageActor()
        {
            _logger.Debug("TweetAverageActor created.");
            Receive<IMyTweetDTO>(HandleTwitterMessageAsync);
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
