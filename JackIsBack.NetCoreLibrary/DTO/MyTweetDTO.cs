using System.Collections.Generic;
using JackIsBack.NetCoreLibrary.Interfaces;

namespace JackIsBack.NetCoreLibrary.DTO
{
    public class MyTweetDTO : IMyTweetDTO
    {
        public long TweetId { get; set; }
        public string Tweet { get; set; }
        public long CreatedById { get; set; }
        public int CurrentTweetCount { get; set; }
        public string CreatedBy { get; set; }
        public bool ContainsEmojis { get; set; }
        public bool ContainsPhotoUrl { get; set; }
        public bool ContainsUrl { get; set; }
        public List<string> TopDomains { get; set; }
        public List<string> TopEmojisUsed { get; set; }
        public List<string> TopHashTags { get; set; }
        public double? AverageTweetsPerHour { get; set; }
        public double? AverageTweetsPerMinute { get; set; }
        public double? AverageTweetsPerSecond { get; set; }
        public double? PercentOfTweetsContainingEmojis { get; set; }
        public double? PercentOfTweetsWithPhotoUrl { get; set; }
        public double? PercentOfTweetsWithUrl { get; set; }
    }
}