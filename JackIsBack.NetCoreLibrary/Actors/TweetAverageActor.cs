using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Commands;
using JackIsBack.NetCoreLibrary.DTO;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class TweetAverageActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();

        public TweetAverageActor()
        {
            _logger.Warning("TweetAverageActor created.");
            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
        }

        private void HandleTwitterMessageAsync(MyTweetDTO myTweetDto)
        {
            var command = new UpdateTweetAverageCommand(1);
            Context.ActorSelection("akka://TwitterStatisticsActorSystem/user/TweetStatisticsActor").Tell(command);
        }
    }
}
