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

        private async void HandleTwitterMessageAsync(ITweet tweet)
        {
            await Task.Factory.StartNew(() =>
            {
                var command = new ChangeTweetQuantityCommand(operation: Operation.Increase, 1);
                var commandManager = new CommandManager();
                commandManager.Invoke(command);

                
                System.Console.WriteLine($"The new Count is: {TweetStatistics.Count} " + tweet.Text);
            });
        }
    }
}
