using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Commands;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Interfaces;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class TotalNumberOfTweetsActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public TotalNumberOfTweetsActor()
        {
            _logger.Debug("TotalNumberOfTweetsActor created.");
            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
        }

        private void HandleTwitterMessageAsync(MyTweetDTO tweet)
        {
            var command = new ChangeTweetQuantityCommand(operation: Operation.Increase, 1);
            Context.ActorSelection("akka://TwitterStatisticsActorSystem/user/TweetStatisticsActor").Tell(command);
        }
    }
}
