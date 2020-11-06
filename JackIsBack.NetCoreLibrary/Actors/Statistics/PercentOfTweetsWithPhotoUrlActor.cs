using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class PercentOfTweetsWithPhotoUrlActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private double PercentOfTweetsWithPhotoUrl { get; set; } = 0.0;

        public PercentOfTweetsWithPhotoUrlActor()
        {
            _logger.Info("PercentOfTweetsWithPhotoUrlActor created.");
            Receive<IMyTweetDTO>(HandleTwitterMessageAsync);
        }

        private void HandleTwitterMessageAsync(IMyTweetDTO message)
        {
            _logger.Debug($"PercentOfTweetsWithPhotoUrlActor got message: {message} ");
        }
    }
}
