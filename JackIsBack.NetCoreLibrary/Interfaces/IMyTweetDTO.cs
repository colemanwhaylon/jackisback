using System.Collections.Generic;

namespace JackIsBack.NetCoreLibrary.Interfaces
{
    public interface IMyTweetDTO
    {
        long TweetId { get; set; }
        string Tweet { get; set; }
        int CurrentTweetCount { get; set; }
        long CreatedById { get; set; }
        string CreatedBy { get; set; }
        bool ContainsEmojis { get; set; }
        bool ContainsPhotoUrl { get; set; }
        bool ContainsUrl { get; set; }
        List<string> TopDomains { get; set; }
        List<string> TopEmojisUsed { get; set; }
        List<string> TopHashTags { get; set; }
        double? AverageTweetsPerHour { get; set; }
        double? AverageTweetsPerMinute { get; set; }
        double? AverageTweetsPerSecond { get; set; }
        double? PercentOfTweetsContainingEmojis { get; set; }
        double? PercentOfTweetsWithPhotoUrl { get; set; }
        double? PercentOfTweetsWithUrl { get; set; }
    }
}