using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class PercentOfTweetsWithUrlActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private double PercentOfTweetsWithPhotoUrl { get; set; } = 0.0;

        public PercentOfTweetsWithUrlActor()
        {
            _logger.Debug("PercentOfTweetsWithUrlActor created.");
            Receive<IMyTweetDTO>(HandleTwitterMessageAsync);
        }

        private void HandleTwitterMessageAsync(IMyTweetDTO message)
        {
            _logger.Debug($"PercentOfTweetsWithUrlActor got message: {message} ");

        }
    }
}
