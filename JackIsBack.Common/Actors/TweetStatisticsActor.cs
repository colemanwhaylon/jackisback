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
            command.Execute();
        }
    }

}