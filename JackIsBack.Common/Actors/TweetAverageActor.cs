using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using Tweetinvi.Models;

namespace JackIsBack.Common.Actors
{
    public class TweetAverageActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();
        private static long Count { get; set; } = 0;
        public TweetAverageActor()
        {
            System.Console.WriteLine("TweetAverageActor created.");
            _logger.Warning("TweetAverageActor created.");
            Receive<ITweet>(HandleTwitterMessageAsync);
        }

        private async void HandleTwitterMessageAsync(ITweet tweet)
        {
            await Task.Factory.StartNew(() =>
            {
                //var command = new ChangeTweetQuantityCommand(operation: Operation.Increase, 1);
                //var commandManager = new CommandManager();
                //commandManager.Invoke(command);

               // System.Console.WriteLine($"TweetAverageActor wrote " + tweet.Text);
                _logger.Warning($"TweetAverageActor wrote " + tweet.IdStr);
            });
        }
    }   
}
