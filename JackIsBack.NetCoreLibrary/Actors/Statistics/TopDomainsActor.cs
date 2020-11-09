using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class TopDomainsActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private static long Count { get; set; } = 0;
        public TopDomainsActor()
        {
            _logger.Debug("TopDomainsActor created.");
            Receive<IMyTweetDTO>(HandleTwitterMessageAsync);
        }

        private void HandleTwitterMessageAsync(IMyTweetDTO message)
        {
            _logger.Debug($"TopDomainsActor got message: {message} ");

        }
    }
}
