using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class PercentOfTweetsWithUrlActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private double? _percentOfTweetsWithUrl { get; set; } = 20.0;

        public PercentOfTweetsWithUrlActor()
        {
            _logger.Debug("PercentOfTweetsWithUrlActor created.");
            Receive<IMyTweetDTO>(HandleTwitterMessageAsync);
            Receive<RefreshStatisticsRequest>(HandleRefreshStatisticsRequest);
            Receive<GetTotalNumberOfTweetsMessage>(HandleGetTotalNumberOfTweetsMessage);
        }
        private void HandleGetTotalNumberOfTweetsMessage(GetTotalNumberOfTweetsMessage message)
        {
            _logger.Debug($"PercentOfTweetsWithUrlActor.HandleGetTotalNumberOfTweetsMessage() got message: {message} Percentage is now: {_percentOfTweetsWithUrl}");
            message.PercentOfTweetsWithUrl = _percentOfTweetsWithUrl;
            Sender.Tell(message, Self);
        }

        private void HandleRefreshStatisticsRequest(RefreshStatisticsRequest message)
        {
            var response = new GetAllStatisticsMessageResponse();
            response.PercentOfTweetsWithUrl = _percentOfTweetsWithUrl;
            Sender.Tell(response, Self);
        }

        private void HandleTwitterMessageAsync(IMyTweetDTO message)
        {
            _logger.Debug($"PercentOfTweetsWithUrlActor got message: {message} ");

        }
    }
}
