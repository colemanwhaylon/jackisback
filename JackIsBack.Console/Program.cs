using Akka.Actor;
using JackIsBack.NetCoreLibrary.Actors;
using JackIsBack.NetCoreLibrary.Interfaces;
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
    public class Program
    {
        private static ActorSystem ActorSystem;

        public static async Task Main(string[] args)
        {
            var command = TweetGeneratorActorCommand.None;
            try
            {
                System.Console.Title = "Twitter Statistics App";
                System.Console.WriteLine("Started Main()!");

                ActorSystem = ActorSystem.Create("TwitterStatisticsActorSystem");

                command = TweetGeneratorActorCommand.StartUp;
                var tweetGeneratorActor = ActorSystem.ActorOf<TweetGeneratorActor>("TweetGeneratorActor");
                tweetGeneratorActor.Tell(command);

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

            System.Console.WriteLine("Program Finished!");
            System.Console.ReadLine();
        }

    }
}