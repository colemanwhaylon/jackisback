using JackIsBack.NetCoreLibrary;
using JackIsBack.NetCoreLibrary.Interfaces;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Akka.Actor;
using JackIsBack.NetCoreLibrary.Actors;

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
            try
            {
                System.Console.Title = "Twitter Statistics App";
                ActorSystem = ActorSystem.Create("TwitterStatisticsActorSystem");
                System.Console.WriteLine("Started Main()!");

                ActorSystem.ActorOf<TweetGenerator>("TweetGenerator").Tell("Run");

                await ActorSystem.WhenTerminated.ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                System.Console.WriteLine($"Message: {exception.Message}\n, StackTrace: {exception.StackTrace}\n, InnerException: {exception.InnerException.Message}");
            }
            finally
            {
                ActorSystem.ActorOf<TweetGenerator>().Tell("Stop");


                ActorSystem.Terminate();

                System.Console.WriteLine("Stopped ActorSystem!");
            }

            System.Console.WriteLine("Program Done!");
            System.Console.ReadLine();
        }

    }
}