using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class TopDomainsActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private static SortedList<string, int>? _topDomains { get; set; }


        public TopDomainsActor()
        {
            _logger.Debug("TopDomainsActor created.");

            _topDomains = new SortedList<string, int>();

            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
            Receive<GetAllStatisticsMessageResponse>(HandleGetAllStatisticsMessageResponse);
        }

        private void HandleTwitterMessageAsync(MyTweetDTO message)
        {
            _logger.Debug($"TopDomainsActor got message: {message} ");
        }

        private void HandleGetAllStatisticsMessageResponse(GetAllStatisticsMessageResponse message)
        {
            message.TopDomains = _topDomains
                .OrderByDescending(r => r.Value)
                .Select(r => r.Key)
                .Take(5)
                .ToList();
            
            TweetStatisticsActor.IActorRefs["TopEmojisUsedActor"].Forward(message);
        }
    }
}
