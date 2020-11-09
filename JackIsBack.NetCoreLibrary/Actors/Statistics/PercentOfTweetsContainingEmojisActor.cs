using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class PercentOfTweetsContainingEmojisActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private double PercentOfTweetsContainingEmojis { get; set; } = 0.0;

        public PercentOfTweetsContainingEmojisActor()
        {
            _logger.Debug("PercentOfTweetsContainingEmojisActor created.");
            Receive<IMyTweetDTO>(HandleTwitterMessageAsync);
        }

        private void HandleTwitterMessageAsync(IMyTweetDTO message)
        {
            _logger.Debug($"PercentOfTweetsContainingEmojisActor got message: {message.Tweet} ");
        }
    }
}
