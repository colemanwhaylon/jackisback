using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Actors;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;
using JackIsBack.NetCoreLibrary.Utility;
using System;
using System.Threading.Tasks;
using Akka.Routing;
using Akka.Util.Internal;

namespace JackIsBack.Console
{
    /// <summary>
    /// This client application will call ITweetGenerator.Run()
    /// to start a Tweet Stream within JackIsBack.NetCoreLibrary to
    /// kickoff results streaming to the console.  The Akka.Net's config
    /// settings drive elastic scaling and processing of all Tweets.
    /// </summary>
    public class TwitterEngine : ReceiveActor
    {
        private static ILoggingAdapter _logger = Context.GetLogger();
        private static ActorSystem ActorSystem;
        private static IActorRef _totalNumberOfTweetsActorRef;
        private static IActorRef _tweetGeneratorActorRef;
        private static IActorRef _routerForAllStatisticActors;

        public TwitterEngine()
        {
            Receive<InitToggleCommandRequest>(HandleTweetEngineActorInitToggleCommand);
            Receive<InitToggleCommandResponse>(HandleInitToggleCommandResponse);
            Receive<RefreshStatisticsRequest>(HandleRefreshStatisticsRequest);
            Receive<GetAllStatisticsMessageResponse>(HandleGetAllStatisticsMessage);
        }

        private void HandleRefreshStatisticsRequest(RefreshStatisticsRequest message)
        {
            _routerForAllStatisticActors.Tell(message);
        }

        private void HandleInitToggleCommandResponse(InitToggleCommandResponse obj)
        {
            
        }

        private async void HandleTweetEngineActorInitToggleCommand(InitToggleCommandRequest message)
        {
            _tweetGeneratorActorRef = Context.ActorOf<TweetGeneratorActor>("TweetGeneratorActor");
            var response =  _tweetGeneratorActorRef.Ask(message).PipeTo(Sender);

            Task.WaitAll(response);

            _routerForAllStatisticActors = Context.ActorOf(
                Props.Empty.WithRouter(
                    new BroadcastGroup(
                        SharedStrings.PercentOfTweetsContainingEmojisActorPath,
                        SharedStrings.PercentOfTweetsWithPhotoUrlActorPath,
                        SharedStrings.PercentOfTweetsWithUrlActorPath,
                        SharedStrings.TopDomainsActorPath,
                        SharedStrings.TopEmojisUsedActorPath,
                        SharedStrings.TopHashTagsActorPath,
                        SharedStrings.TweetAverageActorPath,
                        SharedStrings.TotalNumberOfTweetsActorPath)));
        }

        private void HandleGetAllStatisticsMessage(GetAllStatisticsMessageResponse messageResponse)
        {
            _logger.Debug($"MessageResponse: {messageResponse}");
            Sender.Tell(messageResponse, Self);
        }

        private async void HandleTweetEngineXCommand(InitToggleCommandRequest message)
        {
            var getAllStatisticsMessage = new GetAllStatisticsMessageResponse(-1);
            var statisticsMessage = ActorSystem
                .ActorSelection(SharedStrings.TotalNumberOfTweetsActorPath)
                .Ask<GetAllStatisticsMessageResponse>(getAllStatisticsMessage).PipeTo(_tweetGeneratorActorRef);

            System.Console.WriteLine(
                $"TwitterEngine.GetAllStatisticsMessageResponse(): Yields = {statisticsMessage}");

            await ActorSystem.WhenTerminated.ConfigureAwait(false);

        }

    }
}