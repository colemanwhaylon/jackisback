using System.Threading.Tasks;
using Akka.Actor;
using JackIsBack.Common.Commands;
using Tweetinvi.Models;

namespace JackIsBack.Common.Actors
{
    public class TotalNumberOfTweetsActor : ReceiveActor
    {
        private static long Count { get; set; } = 0;
        public TotalNumberOfTweetsActor()
        {
            System.Console.WriteLine("TotalNumberOfTweetsActor created.");
            Receive<ITweet>(HandleTwitterMessageAsync);
        }

        private void HandleTwitterMessageAsync(ITweet tweet)
        {
            var command = new ChangeTweetQuantityCommand(operation: Operation.Increase, 1);

            Context.ActorSelection("akka://TwitterStatisticsActorSystem/user/TweetStatisticsActor").Tell(command);
        }
    }
}
