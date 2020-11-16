using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Utility;
using Tweetinvi.Core.Extensions;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class TopHashTagsAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();


        public TopHashTagsAnalyzerActor()
        {
            _logger.Debug("TopHashTagsAnalyzerActor created.");

            Receive<MyTweetDTO>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(MyTweetDTO message)
        {
            _logger.Debug($"TopHashTagsAnalyzerActor is analyzing tweet message: {message}");

            //var regex = new Regex(@"#\w+");
            //var matches = regex.Matches(_tweet.Tweet);
            //if (matches.Any())
            //{
            //    long aValue;
            //    foreach (var match in matches)
            //    {
            //        var key = match.ToString();
            //        if (TweetStatistics.HashTags.ContainsKey(key))
            //        {
            //            if (TweetStatistics.HashTags.TryGetValue(key, out aValue))
            //            {
            //                TweetStatistics.HashTags.AddOrUpdate(key, aValue + 1);
            //                _values.Add(new KeyValuePair<string, long>(key, aValue + 1));
            //            }
            //        }
            //        else
            //        {
            //            TweetStatistics.HashTags.TryAdd(key, 1);
            //            _values.Add(new KeyValuePair<string, long>(key, 1));
            //        }
            //    }
            //}

            Context.ActorSelection(SharedStrings.TopHashTagsActorPath).Tell(message);
            //Context.Self.Tell(PoisonPill.Instance);
        }

        //private void HandleUpdateHashTagsCommand(UpdateHashTagsCommand command)
        //{
        //    _logger.Debug($"TweetStatistics.HashTags key count: {TweetStatistics.HashTags.Keys.Count}");
        //    TweetStatistics.HashTags.ToList().Sort((x, y) => x.Value.CompareTo(y.Value));
        //    var list = TweetStatistics.HashTags.OrderByDescending((x) => x.Value).Take(5);
        //    _logger.Debug($"TweetStatistics.HashTags top 5: ");
        //    var count = 0;
        //    foreach (var item in list)
        //    {
        //        count++;
        //        _logger.Debug($"#{count}: {item}");
        //    }

        //}
    }
}