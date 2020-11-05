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
        private int _count = 0;

        public TotalNumberOfTweetsActor()
        {
            _logger.Debug("TotalNumberOfTweetsActor created.");
            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
        }

        private void HandleTwitterMessageAsync(MyTweetDTO tweet)
        {
            var command = new ChangeTotalNumberOfTweetsMessage(operation: Operation.Increase, 1);
            Context.ActorSelection("akka://TwitterStatisticsActorSystem/user/TweetStatisticsActor").Tell(command);
        }
    }
}
