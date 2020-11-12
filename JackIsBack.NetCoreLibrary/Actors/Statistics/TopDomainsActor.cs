using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Event;
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
            Receive<GetAllStatisticsMessageResponse>(HandleTwitterMessageAsync);
            Receive<RefreshStatisticsRequest>(HandleRefreshStatisticsRequest);
            Receive<GetTotalNumberOfTweetsMessage>(HandleGetTotalNumberOfTweetsMessage);
        }

        private void HandleGetTotalNumberOfTweetsMessage(GetTotalNumberOfTweetsMessage message)
        {
            _logger.Debug($"TopDomainsActor.HandleGetTotalNumberOfTweetsMessage() got message: {message} _topDomains Count is now: {_topDomains?.Count}");
            message.TopDomains = _topDomains;
            Sender.Tell(message, Self);
        }

        private void HandleRefreshStatisticsRequest(RefreshStatisticsRequest message)
        {
            var response = new GetAllStatisticsMessageResponse();
            response.TopDomains = _topDomains;
            Sender.Tell(response, Self);
        }

        private void HandleTwitterMessageAsync(GetAllStatisticsMessageResponse messageResponse)
        {
            _logger.Debug($"TopDomainsActor got messageResponse: {messageResponse} ");

            if (messageResponse.TopDomains != null)
            {
                int aValue;
                foreach (var domain in messageResponse.TopDomains)
                {
                    var key = domain.Key;
                    if (_topDomains.ContainsKey(key))
                    {
                        if (_topDomains.TryGetValue(key, out aValue))
                        {
                            _topDomains.TryAdd(key, aValue + 1);
                        }
                    }
                    else
                    {
                        _topDomains.TryAdd(key, 1);
                    }
                }
            }
        }
    }
}
