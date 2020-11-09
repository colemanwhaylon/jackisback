using Akka.Actor;
using JackIsBack.NetCoreLibrary.Actors;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;
using JackIsBack.NetCoreLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.DI.Core;
using JackIsBack.NetCoreLibrary.Actors.Statistics;

namespace JackIsBack.Console
{
    /// <summary>
    /// This client application will call ITweetGenerator.Run()
    /// to start a Tweet Stream within JackIsBack.NetCoreLibrary to
    /// kickoff results streaming to the console.  The Akka.Net's config
    /// settings drive elastic scaling and processing of all Tweets.
    /// </summary>
    public class Program
    {
        private static ActorSystem ActorSystem;
        private static IActorRef _totalNumberOfTweetsActorRef;

        private void HandleGetAllStatisticsMessage(GetAllStatisticsMessage message)
        {
            System.Console.WriteLine($"Total Tweet Count = {message.TotalNumberOfTweets}");
        }

        public static async Task Main(string[] args)
        {
            var command = TweetGeneratorActorCommand.None;
            try
            {
                System.Console.Title = "Twitter Statistics App";
                System.Console.WriteLine("Started Main()!");

                ActorSystem = ActorSystem.Create("TwitterStatisticsActorSystem");

                command = TweetGeneratorActorCommand.StartUp;
                var tweetGeneratorActorRef = ActorSystem.ActorOf<TweetGeneratorActor>("TweetGeneratorActor");
                tweetGeneratorActorRef.Tell(command);
                
                await Task.Delay(10000);

                var getAllStatisticsMessage = new GetAllStatisticsMessage(-1);
                var statisticsMessage = ActorSystem
                    .ActorSelection(SharedStrings.TotalNumberOfTweetsActorPath)
                    .Ask<GetAllStatisticsMessage>(getAllStatisticsMessage).Result;

                System.Console.WriteLine($"Program.GetAllStatisticsMessage(): Yields = {statisticsMessage.TotalNumberOfTweets}");

                await ActorSystem.WhenTerminated.ConfigureAwait(false);

                //var tasks = new List<Task>();
                //GetAllStatisticsMessage retVal = null;
                //var message = new GetAllStatisticsMessage(0);
                //tasks.Add(ActorSystem.ActorSelection(SharedStrings.TweetStatisticsActorPath)
                //    .Ask<GetAllStatisticsMessage>(message));
                //await Task.WhenAll(tasks).PipeTo(tweetGeneratorActorRef);
            }
            catch (Exception exception)
            {
                System.Console.WriteLine($"Message: {exception.Message}\n, StackTrace: {exception.StackTrace}\n, InnerException: {exception?.InnerException?.Message}");
            }
            finally
            {
                command = TweetGeneratorActorCommand.Shutdown;
                ActorSystem.ActorOf<TweetGeneratorActor>().Tell(command);

                await ActorSystem.Terminate();

                System.Console.WriteLine("Stopped ActorSystem!");
            }

            System.Console.WriteLine("Program Finished!");
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