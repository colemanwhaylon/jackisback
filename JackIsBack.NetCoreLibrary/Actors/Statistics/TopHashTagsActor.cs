using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class TopHashTagsActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly SortedList<string, int>? _hashTags;

        public TopHashTagsActor()
        {
            _logger.Debug("TopHashTagsActor created.");
            _hashTags = new SortedList<string, int>();

            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
            Receive<GetAllStatisticsMessageResponse>(HandleGetAllStatisticsMessageResponse);
        }

        private void HandleGetAllStatisticsMessageResponse(GetAllStatisticsMessageResponse message)
        {
            message.TopHashTags= _hashTags
                .OrderByDescending(r => r.Value)
                .Select(r => r.Key)
                .Take(5)
                .ToList();

            TweetStatisticsActor.IActorRefs["TweetAverageActor"].Forward(message);
        }


        private void HandleTwitterMessageAsync(MyTweetDTO message)
        {
            _logger.Debug($"TopHashTagsActor got message: {message} ");

        }
    }
}
