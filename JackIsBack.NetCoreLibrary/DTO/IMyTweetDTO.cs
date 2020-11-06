using System.Collections.Generic;

namespace JackIsBack.NetCoreLibrary.DTO
{
    public interface IMyTweetDTO
    {
        long TweetId { get; set; }
        string Tweet { get; set; }
        long CreatedById { get; set; }
        string CreatedBy { get; set; }
        bool ContainsEmojis { get; set; }
        bool ContainsPhotoUrl { get; set; }
        bool ContainsUrl { get; set; }
        List<string> TopDomains { get; set; }
        List<string> TopEmojisUsed { get; set; }
        List<string> TopHashTags { get; set; }
        double AverageTweetsPerHour { get; set; }
        double AverageTweetsPerMinute { get; set; }
        double AverageTweetsPerSecond { get; set; }
    }
}