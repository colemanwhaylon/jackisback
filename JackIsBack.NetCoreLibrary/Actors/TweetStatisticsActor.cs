using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Commands;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class TweetStatisticsActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public TweetStatisticsActor()
        {
            _logger.Debug("TweetStatisticsActor created.");
            Receive<ChangeTweetQuantityCommand>(HandleIncreaseTweetCountCommand);
            Receive<UpdateTweetAverageCommand>(HandleTweetAverageCommand);
        }

        private void HandleTweetAverageCommand(UpdateTweetAverageCommand command)
        {
            command.Execute();

            _logger.Debug($"Command: {command.ToString()}");
            _logger.Debug($"TweetStatisticsActor.HandleTweetAverageCommand()\n\tTweetStatistics.AverageTweetsPerHour: {TweetStatistics.AverageTweetsPerHour}\n" +
                          $"\tTweetStatistics.AverageTweetsPerMinute: {TweetStatistics.AverageTweetsPerMinute}\n" +
                          $"\tTweetStatistics.AverageTweetsPerSecond: {TweetStatistics.AverageTweetsPerSecond}");
        }

        private void HandleIncreaseTweetCountCommand(ChangeTweetQuantityCommand command)
        {
            command.Execute();

            _logger.Debug($"Command: {command.ToString()}");
            _logger.Debug($"TweetStatistics.TotalTweetCount: {TweetStatistics.TotalTweetCount}");
        }
    }

}