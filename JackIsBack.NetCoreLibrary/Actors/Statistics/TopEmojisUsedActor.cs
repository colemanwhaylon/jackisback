using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;
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
            Receive<IMyTweetDTO>(HandleTwitterMessageAsync);
            Receive<RefreshStatisticsRequest>(HandleRefreshStatisticsRequest);
            Receive<GetTotalNumberOfTweetsMessage>(HandleGetTotalNumberOfTweetsMessage);
        }

        private void HandleGetTotalNumberOfTweetsMessage(GetTotalNumberOfTweetsMessage message)
        {
            _logger.Debug($"TopEmojisUsedActor.HandleGetTotalNumberOfTweetsMessage() got message: {message} _topEmojis Count is now: {_topEmojis?.Count}");
            message.TopEmojis = _topEmojis;
            Sender.Tell(message, Self);
        }

        private void HandleRefreshStatisticsRequest(RefreshStatisticsRequest message)
        {
            var response = new GetAllStatisticsMessageResponse();
            response.TopEmojis = _topEmojis;
            Sender.Tell(response, Self);
        }

        private void HandleTwitterMessageAsync(IMyTweetDTO message)
        {
            _logger.Debug($"TopEmojisUsedActor got message: {message} ");

        }
    }
}
