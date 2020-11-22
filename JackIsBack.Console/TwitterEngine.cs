using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Actors;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;
using System;
using System.Threading.Tasks;

namespace JackIsBack.Console
{
    public class TwitterEngine : ReceiveActor
    {
        private static ILoggingAdapter _logger = Context.GetLogger();
        private static GetAllStatisticsMessageResponse Statistics;
        private static IActorRef _tweetGeneratorActorRef;

        public TwitterEngine()
        {
            Receive<InitToggleCommandRequest>(HandleTweetEngineActorInitToggleCommand);
            Receive<RefreshStatisticsRequest>(HandleRefreshStatisticsRequest);
            Receive<GetAllStatisticsMessageResponse>(HandleGetAllStatisticsMessage);

        }

        private void HandleRefreshStatisticsRequest(RefreshStatisticsRequest message)
        {
            _tweetGeneratorActorRef.Tell(message, Self);
        }

        private async void HandleTweetEngineActorInitToggleCommand(InitToggleCommandRequest message)
        {
            _tweetGeneratorActorRef = Context.ActorOf<TweetGeneratorActor>("TweetGeneratorActor");
            var response = _tweetGeneratorActorRef.Ask(message).PipeTo(Sender);

            Task.WaitAll(response);
        }

        private void HandleGetAllStatisticsMessage(GetAllStatisticsMessageResponse messageResponse)
        {
            _logger.Debug($"MessageResponse: {messageResponse}");
            Statistics = messageResponse;
            
            Context.System.ActorSelection("/user/TwitterConsole").Tell(Statistics, Self);
        }

    }
}