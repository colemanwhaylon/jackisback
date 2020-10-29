using System.Threading.Tasks;
using Akka.Actor;
using JackIsBack.Common.Commands;
using JackIsBack.Common.DTO;
using Tweetinvi.Models;

namespace JackIsBack.Common.Actors
{
    public class PercentOfTweetsContainingEmojisActor : ReceiveActor
    {
        private static long Count { get; set; } = 0;
        public PercentOfTweetsContainingEmojisActor()
        {
            System.Console.WriteLine("PercentOfTweetsContainingEmojisActor created.");
            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
        }

        private async void HandleTwitterMessageAsync(MyTweetDTO tweet)
        {
            await Task.Factory.StartNew(() =>
            {
                //var command = new ChangeTweetQuantityCommand(operation: Operation.Increase, 1);
                //var commandManager = new CommandManager();
                //commandManager.Invoke(command);

                System.Console.WriteLine($"PercentOfTweetsContainingEmojisActor wrote " + tweet);
            });
        }
    }
}
