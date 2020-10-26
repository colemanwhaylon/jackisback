using Akka.Actor;
using Akka.Event;
using JackIsBack.Common.Commands;

namespace JackIsBack.Common.Actors
{
    public class TweetStatisticsActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        public TweetStatisticsActor()
        {
            System.Console.WriteLine("TweetStatisticsActor created.");
            _logger.Info("TweetStatisticsActor created.");
            Receive<ChangeTweetQuantityCommand>(HandleIncreaseTweetCountCommand);
        }

        private void HandleIncreaseTweetCountCommand(ChangeTweetQuantityCommand command)
        {
            //System.Console.WriteLine($"Path: {this.Self}\tThe Count before command.Execute() is: {TweetStatistics.Count} ");
            command.Execute();
            _logger.Info($"Path: {this.Self}\tThe Count after command.Execute() is now: {TweetStatistics.Count} ");

        }
    }

}