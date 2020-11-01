using JackIsBack.NetCoreLibrary;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Akka.Actor;
using JackIsBack.NetCoreLibrary.Actors;
using Tweetinvi.Events;

namespace JackIsBack.Console
{
    /// <summary>
    /// This client application will subscribe to a Tweet Stream within
    /// JackIsBack.Common to display within the console.
    /// </summary>
    public class Program
    {
        private static ActorSystem ActorSystem;

        public static async Task Main(string[] args)
        {
            ActorSystem = ActorSystem.Create("TwitterStatisticsActorSystem");
            IActorRef mainActorRef = ActorSystem.ActorOf<MainActor>("MainActor");
            

            //TweetGenerator tweetGenerator = new TweetGenerator();
            try
            {
              //  await tweetGenerator.Run();
                System.Console.Title = "Twitter Statistics App";
                System.Console.WriteLine("Started Main()!");
                System.Console.ReadLine();
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception);
            }
            finally
            {
                await ActorSystem.Terminate();

                //await tweetGenerator.Stop();
                System.Console.WriteLine("ActorSystem Finished()!");
            }

            System.Console.WriteLine("Program Done!");
            System.Console.ReadLine();
        }

    }
}