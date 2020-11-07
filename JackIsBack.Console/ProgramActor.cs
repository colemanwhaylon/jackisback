using Akka.Actor;
using JackIsBack.NetCoreLibrary.Actors;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;
using JackIsBack.NetCoreLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JackIsBack.Console
{
    /// <summary>
    /// This client application will call ITweetGenerator.Run()
    /// to start a Tweet Stream within JackIsBack.NetCoreLibrary to
    /// kickoff results streaming to the console.  The Akka.Net's config
    /// settings drive elastic scaling and processing of all Tweets.
    /// </summary>
    public class ProgramActor : ReceiveActor
    {
        private static ActorSystem ActorSystem;
        private static GetAllStatisticsMessage Statistics;

        public ProgramActor()
        {
            Receive<GetAllStatisticsMessage>(HandleGetAllStatisticsMessage);
        }

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
                var programActorRef = ActorSystem.ActorOf<ProgramActor>("ProgramActor");

                command = TweetGeneratorActorCommand.StartUp;
                var tweetGeneratorActorRef = ActorSystem.ActorOf<TweetGeneratorActor>("TweetGeneratorActor");
                tweetGeneratorActorRef.Tell(command);

                var tasks = new List<Task>();
                GetAllStatisticsMessage retVal = null;
                var message = new GetAllStatisticsMessage(0);
                tasks.Add(ActorSystem.ActorSelection(SharedStrings.TweetStatisticsActorPath)
                    .Ask<GetAllStatisticsMessage>(message));

                Task.WhenAll(tasks).Wait();

                await ActorSystem.WhenTerminated.ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                System.Console.WriteLine($"Message: {exception.Message}\n, StackTrace: {exception.StackTrace}\n, InnerException: {exception.InnerException.Message}");
            }
            finally
            {
                command = TweetGeneratorActorCommand.Shutdown;
                ActorSystem.ActorOf<TweetGeneratorActor>().Tell(command);

                await ActorSystem.Terminate();

                System.Console.WriteLine("Stopped ActorSystem!");
            }

            System.Console.WriteLine("ProgramActor Finished!");
            System.Console.ReadLine();
        }


    }
}