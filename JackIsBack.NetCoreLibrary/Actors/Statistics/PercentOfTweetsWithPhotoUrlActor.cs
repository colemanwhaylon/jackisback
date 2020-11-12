using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class PercentOfTweetsWithPhotoUrlActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private double? _percentOfTweetsWithPhotoUrl { get; set; } = 15.0;

        public PercentOfTweetsWithPhotoUrlActor()
        {
            _logger.Debug("PercentOfTweetsWithPhotoUrlActor created.");
            Receive<IMyTweetDTO>(HandleTwitterMessageAsync);
            Receive<RefreshStatisticsRequest>(HandleRefreshStatisticsRequest);
            Receive<GetTotalNumberOfTweetsMessage>(HandleGetTotalNumberOfTweetsMessage);
        }

        private void HandleRefreshStatisticsRequest(RefreshStatisticsRequest obj)
        {
            var response = new GetAllStatisticsMessageResponse();
            response.PercentOfTweetsWithPhotoUrl = _percentOfTweetsWithPhotoUrl;
            Sender.Tell(response, Self);
        }
        private void HandleGetTotalNumberOfTweetsMessage(GetTotalNumberOfTweetsMessage message)
        {
            _logger.Debug($"PercentOfTweetsWithPhotoUrlActor.HandleGetTotalNumberOfTweetsMessage() got message: {message} Percentage is now: {_percentOfTweetsWithPhotoUrl}");
            message.PercentOfTweetsWithPhotoUrl = _percentOfTweetsWithPhotoUrl;
            Sender.Tell(message, Self);
        }

        private void HandleTwitterMessageAsync(IMyTweetDTO message)
        {
            _logger.Debug($"PercentOfTweetsWithPhotoUrlActor got message: {message} ");
        }
    }
}
