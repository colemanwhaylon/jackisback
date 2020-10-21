using Akka.Actor;

namespace JackIsBack.Console.Actors
{
    public class TotalNumberOfTweetsActor : ReceiveActor
    {
        public static long Count { get; set; } = 0;
        public TotalNumberOfTweetsActor()
        {
            System.Console.WriteLine("Creating a TotalNumberOfTweetsActor");
            Receive<string>(HandleTwitterMessage);
        }

        private void HandleTwitterMessage(string tweet)
        {
            System.Console.WriteLine($"count: {Count++}\t" + tweet);
        }
    }
}
