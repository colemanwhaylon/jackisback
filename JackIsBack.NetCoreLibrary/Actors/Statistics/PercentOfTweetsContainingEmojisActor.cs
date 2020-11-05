using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class PercentOfTweetsContainingEmojisActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private double PercentOfTweetsContainingEmojis { get; set; } = 0.0;

        public PercentOfTweetsContainingEmojisActor()
        {
            _logger.Info("PercentOfTweetsContainingEmojisActor created.");
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
