using System.Collections.Generic;
using JackIsBack.NetCoreLibrary.Interfaces;

namespace JackIsBack.NetCoreLibrary.DTO
{
    public class MyTweetDTO : IMyTweetDTO
    {
        public long TweetId { get; set; } = 0;
        public string Tweet { get; set; } = string.Empty;
        public long CreatedById { get; set; } = 0;
        public int CurrentTweetCount { get; set; } = 0;
        public string CreatedBy { get; set; } = string.Empty;
        public bool ContainsEmojis { get; set; } = false;
        public bool ContainsPhotoUrl { get; set; } = false;
        public bool ContainsUrl { get; set; } = false;
        public List<string> TopDomains { get; set; } = new List<string>();
        public List<string> TopEmojisUsed { get; set; } = new List<string>();
        public List<string> TopHashTags { get; set; } = new List<string>();
        public double? AverageTweetsPerHour { get; set; } = 0.0;
        public double? AverageTweetsPerMinute { get; set; } = 0.0;
        public double? AverageTweetsPerSecond { get; set; } = 0.0;
        public double? PercentOfTweetsContainingEmojis { get; set; } = 0.0;
        public double? PercentOfTweetsWithPhotoUrl { get; set; } = 0.0;
        public double? PercentOfTweetsWithUrl { get; set; } = 0.0;
    }
}