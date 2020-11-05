using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class PercentOfTweetsWithPhotoUrlActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private double PercentOfTweetsWithPhotoUrl { get; set; } = 0.0;

        public PercentOfTweetsWithPhotoUrlActor()
        {
            _logger.Info("PercentOfTweetsWithPhotoUrlActor created.");
            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
        }

        private async void HandleTwitterMessageAsync(MyTweetDTO tweet)
        {
            await Task.Factory.StartNew(() =>
            {
                //var command = new ChangeTweetQuantityCommand(operation: Operation.Increase, 1);
                //var commandManager = new CommandManager();
                //commandManager.Invoke(command);

                System.Console.WriteLine($"PercentOfTweetsWithPhotoUrlActor wrote " + tweet);
            });
        }
    }
}
