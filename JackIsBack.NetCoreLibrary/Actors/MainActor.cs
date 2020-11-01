using Akka.Actor;
using Akka.Event;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class MainActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();


        public MainActor()
        {
            _logger.Debug("MainActor created.");
            Receive<string>(HandleTwitterMessageAsync);
        }

        private void HandleTwitterMessageAsync(string tweet)
        {
            _logger.Debug(tweet);


            //Context.ActorSelection("akka://TwitterStatisticsActorSystem/user/TweetStatisticsActor").Tell(command);
        }
    }
}