using System;

namespace JackIsBack.NetCoreLibrary.Messages
{
    public class TimeKeeperActorMessage
    {
        public TimeSpan StartDateTime { get; private set; }
        public TimeSpan EndDateTime { get; private set; }

        public TimeKeeperActorMessage(TimeSpan? startDateTime, TimeSpan? endDateTime)
        {
            StartDateTime = startDateTime?? TimeSpan.Zero;
            EndDateTime = endDateTime ?? TimeSpan.Zero;
        }

        public TimeKeeperActorMessage(long? startDateTimeTicks, long? endDateTimeTicks)
        {
            StartDateTime = startDateTimeTicks.HasValue ? new TimeSpan(startDateTimeTicks.Value) :  new TimeSpan( TimeSpan.Zero.Ticks);
            EndDateTime = endDateTimeTicks.HasValue ? new TimeSpan(endDateTimeTicks.Value) : new TimeSpan(TimeSpan.Zero.Ticks);
        }
    }
}