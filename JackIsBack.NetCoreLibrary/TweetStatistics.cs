using System;
using System.Collections.Generic;

namespace JackIsBack.NetCoreLibrary
{
    public static class TweetStatistics
    {
        
        public static long TotalTweetCount = 0;
        public static long AverageTweetsPerHour { get; set; } = 0;
        public static long AverageTweetsPerMinute { get; set; } = 0;
        public static long AverageTweetsPerSecond { get; set; } = 0;
        public static Dictionary<string, long> HashTags { get; set; }
    }
}