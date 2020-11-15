using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class TopEmojisUsedActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private static SortedList<string, int>? _topEmojis { get; set; }

        public TopEmojisUsedActor()
        {
            _logger.Debug("TopEmojisUsedActor created.");
            _topEmojis = new SortedList<string, int>();

            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
            Receive<GetAllStatisticsMessageResponse>(HandleGetAllStatisticsMessageResponse);
        }

        private void HandleGetAllStatisticsMessageResponse(GetAllStatisticsMessageResponse message)
        {
            message.TopEmojis = _topEmojis
                .OrderByDescending(r => r.Value)
                .Select(r => r.Key)
                .Take(5)
                .ToList();

            TweetStatisticsActor.IActorRefs["TopHashTagsActor"].Forward(message);
        }


        private void HandleTwitterMessageAsync(MyTweetDTO message)
        {
            _logger.Debug($"TopEmojisUsedActor got message: {message} ");

        }
    }
}
