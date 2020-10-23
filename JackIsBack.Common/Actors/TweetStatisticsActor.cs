using Akka.Actor;
using JackIsBack.Common.Commands;

namespace JackIsBack.Common.Actors
{
    public class TweetStatisticsActor : ReceiveActor
    {
        public TweetStatisticsActor()
        {
            System.Console.WriteLine("TweetStatisticsActor created.");
            Receive<ChangeTweetQuantityCommand>(HandleIncreaseTweetCountCommand);
        }

        private void HandleIncreaseTweetCountCommand(ChangeTweetQuantityCommand command)
        {
            //System.Console.WriteLine($"Path: {this.Self}\tThe Count before command.Execute() is: {TweetStatistics.Count} ");
            command.Execute();
            System.Console.WriteLine($"Path: {this.Self}\tThe Count after command.Execute() is now: {TweetStatistics.Count} " );
        }
    }

}