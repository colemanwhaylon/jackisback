using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
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

            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
            Receive<GetAllStatisticsMessageResponse>(HandleGetAllStatisticsMessageResponse);
        }
        private void HandleGetAllStatisticsMessageResponse(GetAllStatisticsMessageResponse message)
        {
            message.PercentOfTweetsWithUrl = _percentOfTweetsWithUrl;
            TweetStatisticsActor.IActorRefs["TopDomainsActor"].Forward(message);
        }

        private void HandleTwitterMessageAsync(MyTweetDTO message)
        {
            _logger.Debug($"PercentOfTweetsWithUrlActor got message: {message} ");
        }
    }
}
