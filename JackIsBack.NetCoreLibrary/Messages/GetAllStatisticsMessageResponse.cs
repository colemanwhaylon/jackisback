using System.Collections.Generic;

namespace JackIsBack.NetCoreLibrary.Messages
{
    public class GetAllStatisticsMessageResponse
    {
        public int? TotalNumberOfTweets { get; private set; } = 0;
        public double? PercentOfTweetsContainingEmojis { get; set; } = 0.0;
        public double? PercentOfTweetsWithPhotoUrl { get; set; } = 0.0;
        public double? PercentOfTweetsWithUrl { get; set; } = 0.0;
        public SortedList<string, int>? TopEmojis { get; set; }
        public SortedList<string, int>? TopDomains { get; set; }
        public SortedList<string, int>? TopHashTags { get; set; }
        public double? AverageTweetsPerHour { get; set; } = 0.0;
        public double? AverageTweetsPerMinute { get; set; } = 0.0;
        public double? AverageTweetsPerSecond { get; set; } = 0.0;
        

        public GetAllStatisticsMessageResponse()
        {
            TopHashTags = new SortedList<string, int>();
            TopDomains = new SortedList<string, int>();
            TopEmojis = new SortedList<string, int>();
        }

        public GetAllStatisticsMessageResponse(int? totalNumberOfTweets = 0, double? percentOfTweetsContainingEmojis = 0, double? percentOfTweetsWithPhotoUrl = 0, double? percentOfTweetsWithUrl = 0, SortedList<string, int>? topDomains = null)
        : this()
        {
            TotalNumberOfTweets = totalNumberOfTweets ?? 0;
            PercentOfTweetsContainingEmojis = percentOfTweetsContainingEmojis ?? 0;
            PercentOfTweetsWithPhotoUrl = percentOfTweetsWithPhotoUrl ?? 0;
            PercentOfTweetsWithUrl = percentOfTweetsWithUrl ?? 0;
            TopDomains = topDomains;
        }

        public override string ToString()
        {
            return $"GetAllStatisticsMessageResponse.TotalNumberOfTweets = {TotalNumberOfTweets}";
        }
    }
}