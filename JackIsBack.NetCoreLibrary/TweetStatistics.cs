using System;

namespace JackIsBack.NetCoreLibrary
{
    public static class TweetStatistics
    {
        public static TimeSpan StartDateTime { get; set; }
        public static TimeSpan EndDateTime { get; set; }
        public static long TotalTweetCount = 0;
        public static long AverageTweetsPerHour { get; set; } = 0;
        public static long AverageTweetsPerMinute { get; set; } = 0;
        public static long AverageTweetsPerSecond { get; set; } = 0;
    }
}