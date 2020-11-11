using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class PercentOfTweetsWithUrlActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private double? PercentOfTweetsWithUrl { get; set; } = 20.0;

        public PercentOfTweetsWithUrlActor()
        {
            _logger.Debug("PercentOfTweetsWithUrlActor created.");
            Receive<IMyTweetDTO>(HandleTwitterMessageAsync);
            Receive<RefreshStatisticsRequest>(HandleRefreshStatisticsRequest);
        }

        private void HandleRefreshStatisticsRequest(RefreshStatisticsRequest obj)
        {
            var response = new GetAllStatisticsMessageResponse();
            response.PercentOfTweetsWithUrl = PercentOfTweetsWithUrl;
            Sender.Tell(response, Self);
        }

        private void HandleTwitterMessageAsync(IMyTweetDTO message)
        {
            _logger.Debug($"PercentOfTweetsWithUrlActor got message: {message} ");

        }
    }
}
