using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Commands;
using JackIsBack.NetCoreLibrary.DTO;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class TopHashTagsActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public TopHashTagsActor()
        {
            _logger.Debug("TopHashTagsActor created.");
            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
        }

        private void HandleTwitterMessageAsync(MyTweetDTO tweet)
        {
            var command = new UpdateHashTagsCommand(tweet);
            Context.ActorSelection("akka://TwitterStatisticsActorSystem/user/TweetStatisticsActor").Tell(command);
        }
    }
}
