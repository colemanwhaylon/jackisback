using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;
using JackIsBack.NetCoreLibrary.Utility;
using Tweetinvi.Core.Extensions;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class TopDomainsAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly List<string> _domains;

        public TopDomainsAnalyzerActor()
        {
            _domains = new List<string>();
            _logger.Debug("TopDomainsAnalyzerActor created.");

            Receive<IMyTweetDTO>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(IMyTweetDTO message)
        {
            _logger.Debug($"TopDomainsAnalyzerActor is analyzing tweet message: {message.Tweet}");

            var regex = new Regex(@"#\w+");
            var matches = regex.Matches(message.Tweet);
            if (matches.Any())
            {
                foreach (var match in matches)
                {
                    var key = match.ToString();
                    _domains.Add(key);
                }
            }

            var result = new GetAllStatisticsMessageResponse(domains: _domains);
            Context.ActorSelection(SharedStrings.TopDomainsActorPath).Tell(result);


            //Context.Self.Tell(PoisonPill.Instance);
        }
    }
}