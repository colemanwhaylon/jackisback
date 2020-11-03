using JackIsBack.NetCoreLibrary;
using JackIsBack.NetCoreLibrary.Interfaces;
using System;
using System.Threading.Tasks;

namespace JackIsBack.Console
{
    /// <summary>
    /// This client application will call ITweetGenerator.RunAsync()
    /// to start a Tweet Stream within JackIsBack.NetCoreLibrary to
    /// kickoff results streaming to the console.  The Akka.Net's config
    /// settings drive elastic scaling and processing of all Tweets.
    /// </summary>
    public class Program
    {
        public static async Task Main(string[] args)
        {
            System.Console.Title = "Twitter Statistics App";
            System.Console.WriteLine("Started Main()!");

            ITweetGenerator tweetGenerator = new TweetGenerator();
            try
            {
                await tweetGenerator.RunAsync();
            }
            catch (Exception exception)
            {
                System.Console.WriteLine($"Message: {exception.Message}\n, StackTrace: {exception.StackTrace}\n, InnerException: {exception.InnerException.Message}");
            }
            finally
            {
                await tweetGenerator.StopAsync();
                System.Console.WriteLine("Stopped ActorSystem!");
            }

            System.Console.WriteLine("Program Done!");
            System.Console.ReadLine();
        }

    }
}