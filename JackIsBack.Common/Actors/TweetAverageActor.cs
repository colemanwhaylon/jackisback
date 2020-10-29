using Akka.Actor;
using Akka.Event;
using JackIsBack.Common.Commands;
using JackIsBack.Common.DTO;

namespace JackIsBack.Common.Actors
{
    public class TweetAverageActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();

        public TweetAverageActor()
        {
            System.Console.WriteLine("TweetAverageActor created.");
            _logger.Warning("TweetAverageActor created.");
            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
        }

        private async void HandleTwitterMessageAsync(MyTweetDTO myTweetDto)
        {
            var command = new UpdateTweetAverageCommand(1);
            _logger.Info($"Command: {command.ToString()}");
            Context.ActorSelection("akka://TwitterStatisticsActorSystem/user/TweetStatisticsActor").Tell(command);


            _logger.Info($"TweetAverageActor wrote \tTweetStatistics.AverageTweetsPerHour: {TweetStatistics.AverageTweetsPerHour}\n" +
                $"\tTweetStatistics.AverageTweetsPerMinute: {TweetStatistics.AverageTweetsPerMinute}\n" +
                $"\tTweetStatistics.AverageTweetsPerSecond: {TweetStatistics.AverageTweetsPerSecond}");
        }
    }
}
