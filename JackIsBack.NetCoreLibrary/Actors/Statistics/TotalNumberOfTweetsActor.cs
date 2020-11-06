using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class TotalNumberOfTweetsActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private int _totalNumberOfTweets = 0;

        public TotalNumberOfTweetsActor()
        {
            _logger.Debug("TotalNumberOfTweetsActor created.");
            Receive<ChangeTotalNumberOfTweetsMessage>(HandleChangeTotalNumberOfTweetsMessage);
        }

        private void HandleChangeTotalNumberOfTweetsMessage(ChangeTotalNumberOfTweetsMessage message)
        {
            switch (message.Operation)
            {
                case Operation.Increase:
                    _totalNumberOfTweets += message.Total;
                    break;
                case Operation.Decrease:
                    _totalNumberOfTweets -= message.Total;
                    break;
            }

            if (message.NeedsResponse)
            {
                var retVal = new ChangeTotalNumberOfTweetsMessage(message.Operation, message.Total, _totalNumberOfTweets, message.NeedsResponse);
                Sender.Tell(retVal);
            }
        }
    }
}
