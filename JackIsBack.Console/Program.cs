using System;
using System.Threading.Tasks;
using JackIsBack.Common;
using Tweetinvi.Events;

namespace JackIsBack.Console
{
    /// <summary>
    /// This client application will subscribe to a Tweet Stream within
    /// JackIsBack.Common to display within the console.
    /// </summary>
    public class Program
    {
        public static async Task Main(string[] args)
        {


            TweetGenerator tweetGenerator = new TweetGenerator(); 
            try
            {
                await tweetGenerator.Run();

                System.Console.WriteLine("Started Main()!");
                System.Console.ReadLine();
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception);
            }
            finally
            {
                await tweetGenerator.Stop();
                System.Console.WriteLine("Finished Main()!");
            }

            System.Console.WriteLine("Program Done!");
            System.Console.ReadLine();
        }

        private static void TweetGeneratorOnTweetReceived(object? sender, TweetReceivedEventArgs e)
        {
            System.Console.WriteLine(e.Tweet.Text);
        }
    }
}