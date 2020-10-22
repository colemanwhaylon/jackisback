using Akka.Actor;
using Tweetinvi.Models;

namespace JackIsBack.Common.Actors
{
    public class TotalNumberOfTweetsActor : ReceiveActor
    {
        private static long Count { get; set; } = 0;
        public TotalNumberOfTweetsActor()
        {
            System.Console.WriteLine("TotalNumberOfTweetsActor created.");
            Receive<ITweet>(HandleTwitterMessage);
        }

        private void HandleTwitterMessage(ITweet tweet)
        {
            System.Console.WriteLine($"count: {Count++}\t" + tweet.Text);
        }
    }
}
