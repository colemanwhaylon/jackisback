using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class PercentOfTweetsContainingEmojisActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private double? _percentOfTweetsContainingEmojis { get; set; } = 10.0;

        public PercentOfTweetsContainingEmojisActor()
        {
            _logger.Debug("PercentOfTweetsContainingEmojisActor created.");
            Receive<IMyTweetDTO>(HandleTwitterMessageAsync); 
            Receive<RefreshStatisticsRequest>(HandleRefreshStatisticsRequest);
            Receive<GetTotalNumberOfTweetsMessage>(HandleGetTotalNumberOfTweetsMessage);
        }

        private void HandleGetTotalNumberOfTweetsMessage(GetTotalNumberOfTweetsMessage message)
        {
            _logger.Debug($"PercentOfTweetsContainingEmojisActor.HandleGetTotalNumberOfTweetsMessage() got message: {message} Percentage is now: {_percentOfTweetsContainingEmojis}");
            message.PercentOfTweetsContainingEmojis = _percentOfTweetsContainingEmojis;
            Sender.Tell(message, Self);
        }

        private void HandleRefreshStatisticsRequest(RefreshStatisticsRequest obj)
        {
            var response = new GetAllStatisticsMessageResponse();
            response.PercentOfTweetsContainingEmojis = _percentOfTweetsContainingEmojis;
            Sender.Tell(response, Self);
        }

        private void HandleTwitterMessageAsync(IMyTweetDTO message)
        {

            _logger.Debug($"PercentOfTweetsContainingEmojisActor got message: {message.Tweet} ");
        }
    }
}
