using Akka.Actor;
using Tweetinvi.Models;

namespace JackIsBack.Console.Actors
{
    public class TotalNumberOfTweetsActor : ReceiveActor
    {
        public static long Count { get; set; } = 0;
        public TotalNumberOfTweetsActor()
        {
            System.Console.WriteLine("Creating a TweetCounterActor");
            Receive<ITweet>(HandleTwitterMessage);
        }

        private void HandleTwitterMessage(ITweet message)
        {
            System.Console.WriteLine($"count: {Count++}\t" + message.Text);
        }
    }
}
