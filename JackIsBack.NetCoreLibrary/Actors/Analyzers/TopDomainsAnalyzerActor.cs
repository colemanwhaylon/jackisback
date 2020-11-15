using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;
using JackIsBack.NetCoreLibrary.Utility;
using Tweetinvi.Core.Extensions;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class TopDomainsAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly SortedList<string, int> _domains;

        public TopDomainsAnalyzerActor()
        {
            _domains = new SortedList<string, int>();
            _logger.Debug("TopDomainsAnalyzerActor created.");

            Receive<MyTweetDTO>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(MyTweetDTO message)
        {
            //_logger.Debug($"TopDomainsAnalyzerActor is analyzing tweet message: {message.Tweet}");

            //var regex = new Regex(@"#\w+");
            //var matches = regex.Matches(message.Tweet);
            //if (matches.Any())
            //{
            //    foreach (var match in matches)
            //    {
            //        var key = match.ToString();
            //        _domains.Add(key, 1);
            //    }
            //}

            //var result = new GetAllStatisticsMessageResponse();
            //Context.ActorSelection(SharedStrings.TopDomainsActorPath).Tell(result);


            //Context.Self.Tell(PoisonPill.Instance);
        }
    }
}