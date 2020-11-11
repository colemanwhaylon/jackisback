using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class PercentOfTweetsContainingEmojisActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private double? PercentOfTweetsContainingEmojis { get; set; } = 10.0;

        public PercentOfTweetsContainingEmojisActor()
        {
            _logger.Debug("PercentOfTweetsContainingEmojisActor created.");
            Receive<IMyTweetDTO>(HandleTwitterMessageAsync); 
            Receive<RefreshStatisticsRequest>(HandleRefreshStatisticsRequest);
        }

        private void HandleRefreshStatisticsRequest(RefreshStatisticsRequest obj)
        {
            var response = new GetAllStatisticsMessageResponse();
            response.PercentOfTweetsContainingEmojis = PercentOfTweetsContainingEmojis;
            Sender.Tell(response, Self);
        }

        private void HandleTwitterMessageAsync(IMyTweetDTO message)
        {
            _logger.Debug($"PercentOfTweetsContainingEmojisActor got message: {message.Tweet} ");
        }
    }
}
