using Akka.Actor;
using Akka.Event;
using JackIsBack.Common.Commands;
using JackIsBack.Common.DTO;
using JackIsBack.Common.Interfaces;
using Tweetinvi.Models;

namespace JackIsBack.Common.Actors
{
    public class TotalNumberOfTweetsActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private static long Count { get; set; } = 0;
        public TotalNumberOfTweetsActor()
        {
            System.Console.WriteLine("TotalNumberOfTweetsActor created.");
            _logger.Info("TotalNumberOfTweetsActor created.");
            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
        }

        private void HandleTwitterMessageAsync(MyTweetDTO tweet)
        {
            var command = new ChangeTweetQuantityCommand(operation: Operation.Increase, 1);
            _logger.Info($"Command: {command.ToString()}");
            Context.ActorSelection("akka://TwitterStatisticsActorSystem/user/TweetStatisticsActor").Tell(command);
        }
    }
}
