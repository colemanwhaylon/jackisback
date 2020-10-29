using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.DTO;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class TopDomainsActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private static long Count { get; set; } = 0;
        public TopDomainsActor()
        {
            _logger.Info("TopDomainsActor created.");
            Receive<MyTweetDTO>(HandleTwitterMessageAsync);
        }

        private async void HandleTwitterMessageAsync(MyTweetDTO tweet)
        {
            await Task.Factory.StartNew(() =>
            {
                //var command = new ChangeTweetQuantityCommand(operation: Operation.Increase, 1);
                //var commandManager = new CommandManager();
                //commandManager.Invoke(command);

                System.Console.WriteLine($"TopDomainsActor wrote " + tweet);
            });
        }
    }
}
