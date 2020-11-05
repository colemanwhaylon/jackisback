using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;
using System.Linq;
using System.Collections;
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
            Receive<UpdateHashTagsCommand>(HandleUpdateHashTagsCommand);
            Receive<string>(HandleTweet);
        }

        private void HandleTweet(string message)
        {
            _logger.Debug($"TweetStatistics got tweet message: {message} from {Sender.Path}");
        }

        private void HandleUpdateHashTagsCommand(UpdateHashTagsCommand command)
        {
            command.Execute();

            _logger.Debug($"TweetStatistics.HashTags key count: {TweetStatistics.HashTags.Keys.Count}");
            TweetStatistics.HashTags.ToList().Sort((x,y)=> x.Value.CompareTo(y.Value));
            var list = TweetStatistics.HashTags.OrderByDescending((x) => x.Value).Take(5);
            _logger.Debug($"TweetStatistics.HashTags top 5: ");
            var count = 0;
            foreach (var item in list)
            {
                count++;
                _logger.Debug($"#{count}: {item}");
            }

        }

        private static int CompareLongs(KeyValuePair<string, long> a, KeyValuePair<string, long> b)
        {
            return a.Value.CompareTo(b.Value);
        }

        private void HandleTweetAverageCommand(UpdateTweetAverageCommand command)
        {
            command.Execute();

            //_logger.Debug($"Command: {command.ToString()}");
            _logger.Debug($"TweetStatistics.AverageTweetsPerHour: {TweetStatistics.AverageTweetsPerHour}\n" +
                          $"\tTweetStatistics.AverageTweetsPerMinute: {TweetStatistics.AverageTweetsPerMinute}\n" +
                          $"\tTweetStatistics.AverageTweetsPerSecond: {TweetStatistics.AverageTweetsPerSecond}");
        }

        private void HandleIncreaseTweetCountCommand(ChangeTweetQuantityCommand command)
        {
            command.Execute();

            //_logger.Debug($"Command: {command.ToString()}");
            _logger.Debug($"TweetStatistics.TotalTweetCount: {TweetStatistics.TotalTweetCount}");
        }
    }

}