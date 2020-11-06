using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class TopEmojisUsedActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private static long Count { get; set; } = 0;
        public TopEmojisUsedActor()
        {
            _logger.Info("TopEmojisUsedActor created.");
            Receive<IMyTweetDTO>(HandleTwitterMessageAsync);
        }

        private void HandleTwitterMessageAsync(IMyTweetDTO message)
        {
            _logger.Debug($"TopEmojisUsedActor got message: {message} ");

        }
    }
}
