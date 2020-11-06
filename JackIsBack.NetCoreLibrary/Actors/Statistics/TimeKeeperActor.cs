using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Messages;
using System;

namespace JackIsBack.NetCoreLibrary.Actors.Statistics
{
    public class TimeKeeperActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private TimeSpan _startDateTime { get; set; }
        private TimeSpan _endDateTime { get; set; }

        public TimeKeeperActor()
        {
            _logger.Debug("TimeKeeperActor created.");
            Receive<TimeKeeperActorMessage>(HandleTimeKeeperActorMessage);
        }

        private void HandleTimeKeeperActorMessage(TimeKeeperActorMessage message)
        {
            _startDateTime = message.StartDateTime;
            _endDateTime = message.EndDateTime;
        }
    }
}