using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class TopHashTagsAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public TopHashTagsAnalyzerActor()
        {
            _logger.Debug("TopHashTagsAnalyzerActor created.");

            Receive<IMyTweetDTO>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(IMyTweetDTO message)
        {
            _logger.Debug($"TopHashTagsAnalyzerActor is analyzing tweet message: {message}");

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