using System;
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
            Receive<UpdateTweetAverageCommand>(HandleTweetAverageCommand);
        }

        private void HandleTweetAverageCommand(UpdateTweetAverageCommand command)
        {
            command.Execute();
        }

        private void HandleIncreaseTweetCountCommand(ChangeTweetQuantityCommand command)
        {
            command.Execute();
        }
    }

}