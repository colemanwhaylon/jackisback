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
        private static SortedList<string, int> TopDomains { get; set; } 
        

        public TopDomainsActor()
        {
            _logger.Debug("TopDomainsActor created.");

            TopDomains = new SortedList<string, int>();
            Receive<GetAllStatisticsMessageResponse>(HandleTwitterMessageAsync);
        }

        private void HandleTwitterMessageAsync(GetAllStatisticsMessageResponse messageResponse)
        {
            _logger.Debug($"TopDomainsActor got messageResponse: {messageResponse} ");

            if (messageResponse.Domains != null)
            {
                int aValue;
                foreach (var domains in messageResponse.Domains)
                {
                    var key = domains;
                    if (TopDomains.ContainsKey(key))
                    {
                        if (TopDomains.TryGetValue(key, out aValue))
                        {
                            TopDomains.TryAdd(key, aValue + 1);
                        }
                    }
                    else
                    {
                        TopDomains.TryAdd(key, 1);
                    }
                }
            }
        }
    }
}
