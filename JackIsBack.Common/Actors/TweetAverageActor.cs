using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Tweetinvi.Models;

namespace JackIsBack.Common.Actors
{
    public class TweetAverageActor : ReceiveActor
    {
        private static long Count { get; set; } = 0;
        public TweetAverageActor()
        {
            System.Console.WriteLine("TweetAverageActor created.");
            Receive<ITweet>(HandleTwitterMessageAsync);
        }

        private async void HandleTwitterMessageAsync(ITweet tweet)
        {
            await Task.Factory.StartNew(() =>
            {
                //var command = new ChangeTweetQuantityCommand(operation: Operation.Increase, 1);
                //var commandManager = new CommandManager();
                //commandManager.Invoke(command);

                System.Console.WriteLine($"TweetAverageActor wrote " + tweet.Text);
            });
        }
    }
}
