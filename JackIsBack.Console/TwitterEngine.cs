using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Actors;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;
using JackIsBack.NetCoreLibrary.Utility;
using System;
using System.Threading.Tasks;

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

        public TwitterEngine()
        {
            Receive<TweetGeneratorActorCommandResponse>(HandleTweetGeneratorActorCommandResponse);
        }

        private async void HandleTweetGeneratorActorCommandResponse(TweetGeneratorActorCommandResponse obj)
        {
            var getAllStatisticsMessage = new GetAllStatisticsMessage(-1);
            var statisticsMessage = ActorSystem
                .ActorSelection(SharedStrings.TotalNumberOfTweetsActorPath)
                .Ask<GetAllStatisticsMessage>(getAllStatisticsMessage).PipeTo(_tweetGeneratorActorRef);

            System.Console.WriteLine(
                $"TwitterEngine.GetAllStatisticsMessage(): Yields = {statisticsMessage}");

            await ActorSystem.WhenTerminated.ConfigureAwait(false);

        }

        private void HandleGetAllStatisticsMessage(GetAllStatisticsMessage message)
        {
            System.Console.WriteLine($"Total Tweet Count = {message.TotalNumberOfTweets}");
        }

        public static async Task<TweetGeneratorActorCommandResponse> Execute(string[] args)
        {
            var command = TweetGeneratorActorCommand.None;
            var result = TweetGeneratorActorCommandResponse.None;
            try
            {
                _logger.Debug("Started TwitterEngine.Execute()!");

                command = TweetGeneratorActorCommand.StartUp;
                _tweetGeneratorActorRef = Context.ActorOf<TweetGeneratorActor>("TweetGeneratorActor");
                result = await _tweetGeneratorActorRef.Ask<TweetGeneratorActorCommandResponse>(command);
            }
            catch (Exception exception)
            {
                System.Console.WriteLine($"Message: {exception.Message}\n, StackTrace: {exception.StackTrace}\n, InnerException: {exception?.InnerException?.Message}");
            }
            return result;

            //finally
            //{
            //    command = TweetGeneratorActorCommand.Shutdown;
            //    ActorSystem.ActorOf<TweetGeneratorActor>().Tell(command);

            //    await ActorSystem.Terminate();

            //    System.Console.WriteLine("Stopped ActorSystem!");
            //}

            System.Console.WriteLine("TwitterEngine Finished!");
            System.Console.ReadLine();
        }

        private static async Task<GetAllStatisticsMessage> RefreshStatisticsAsync()
        {
            GetAllStatisticsMessage retVal = null;
            var message = new GetAllStatisticsMessage(0);
            var myStats = ActorSystem.ActorSelection(SharedStrings.TweetStatisticsActorPath)
                .Ask<GetAllStatisticsMessage>(message);
            Task.WaitAll(myStats);

            return myStats.Result;
        }

        /*
         *  private static void RefreshScreen()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var token = cts.Token;

            Task t = Task.Factory.StartNew(
                () =>
                {
                    MainLoop(token);
                },
                token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default
            );

            System.Console.ReadLine();
            cts.Cancel();

            try
            {
                t.Wait();
            }
            catch (AggregateException ae)
            {
                // catch inner exception 
            }
            catch (Exception crap)
            {
                // catch something else
            }
        }

        static async void MainLoop(CancellationToken token)
        {
            while (true)
            {
                // Poll on this property if you have to do 
                // other cleanup before throwing. 
                if (token.IsCancellationRequested)
                {
                    // Clean up here, then...
                    //  "cleanup".Dump();
                    token.ThrowIfCancellationRequested();
                }
                // do something here.
                try
                {
                    var statistics = await RefreshStatisticsAsync();
                    System.Console.Write(statistics);
                }
                catch { }

                Thread.Sleep(5000);
            }
        }

        private static async Task<GetAllStatisticsMessage> RefreshStatisticsAsync()
        {
            GetAllStatisticsMessage retVal = null;
            var message = new GetAllStatisticsMessage(0);
            var myStats = ActorSystem.ActorSelection(SharedStrings.TweetStatisticsActorPath)
                .Ask<GetAllStatisticsMessage>(message);
            Task.WaitAll(myStats);

            return myStats.Result;
        }
         *
         *
         *
         *
         *
         *
         *
         */
    }
}