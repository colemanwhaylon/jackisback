using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
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
            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
            Receive< GetAllStatisticsMessageResponse>(HandleGetAllStatisticsMessageResponse);
        }

        private void HandleGetAllStatisticsMessageResponse(GetAllStatisticsMessageResponse message)
        {
            message.PercentOfTweetsWithPhotoUrl = _percentOfTweetsWithPhotoUrl;
            TweetStatisticsActor.IActorRefs["PercentOfTweetsWithUrlActor"].Forward(message);
        }

        private void HandleTwitterMessageAsync(MyTweetDTO message)
        {
            _percentOfTweetsWithPhotoUrl = message.PercentOfTweetsWithPhotoUrl;
        }
    }
}
