using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Akka.Actor;
using Akka.Event;
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

            Receive<IMyTweetDTO>(HandleTwitterMessageAsync);
            Receive<RefreshStatisticsRequest>(HandleRefreshStatisticsRequest);
            Receive<GetTotalNumberOfTweetsMessage>(HandleGetTotalNumberOfTweetsMessage);
        }
        private void HandleGetTotalNumberOfTweetsMessage(GetTotalNumberOfTweetsMessage message)
        {
            _logger.Debug($"TopHashTagsActor.HandleGetTotalNumberOfTweetsMessage() got message: {message} Percentage is now: {_hashTags}");
            message.TopHashTags = _hashTags;
            Sender.Tell(message, Self);
        }

        private void HandleRefreshStatisticsRequest(RefreshStatisticsRequest message)
        {
            _logger.Debug($"TopHashTagsActor.HandleRefreshStatisticsRequest() got message: {message}.  Count is now: {_hashTags?.Count}");
            var result = new GetAllStatisticsMessageResponse();
            var tempList = _hashTags
                .OrderByDescending(r => r.Value)
                .Take(10);

            result?.TopHashTags?.Clear();
            foreach (var keyValuePair in tempList)
            {
                result?.TopHashTags?.Add(keyValuePair.Key, keyValuePair.Value);
            }

            Sender.Tell(result, Self);
        }

        private void HandleTwitterMessageAsync(IMyTweetDTO message)
        {
            //Context.ActorSelection("akka://TwitterStatisticsActorSystem/user/TweetStatisticsActor").Tell(command);
            _logger.Debug($"TopHashTagsActor got message: {message} ");


        }
    }
}
