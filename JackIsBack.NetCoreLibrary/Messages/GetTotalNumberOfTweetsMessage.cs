using System.Collections.Generic;

namespace JackIsBack.NetCoreLibrary.Messages
{
    public class GetTotalNumberOfTweetsMessage
    {
        public GetTotalNumberOfTweetsMessage()
        {
            TopDomains = new SortedList<string, int>();
            TopEmojis = new SortedList<string, int>();
            TopHashTags = new SortedList<string, int>();
        }

        public GetTotalNumberOfTweetsMessage(int? totalNumberOfTweets = 0, double? percentOfTweetsContainingEmojis = 0,
            double? percentOfTweetsWithPhotoUrl = 0, double? percentOfTweetsWithUrl = 0, double? averageTweetsPerHour = 0,
            double? averageTweetsPerMinute = 0, double? averageTweetsPerSecond = 0)
        : this()
        {
            TotalNumberOfTweets = totalNumberOfTweets ?? 0;
            PercentOfTweetsContainingEmojis = percentOfTweetsContainingEmojis ?? 0;
            PercentOfTweetsWithPhotoUrl = percentOfTweetsWithPhotoUrl;
            PercentOfTweetsWithUrl = percentOfTweetsWithUrl;
            AverageTweetsPerHour = averageTweetsPerHour;
            AverageTweetsPerMinute = averageTweetsPerMinute;
            AverageTweetsPerSecond = averageTweetsPerSecond;
        }
        public int TotalNumberOfTweets { get; private set; }
        public double? PercentOfTweetsContainingEmojis { get; set; }
        public double? PercentOfTweetsWithPhotoUrl { get; set; }
        public double? PercentOfTweetsWithUrl { get; set; }
        public double? AverageTweetsPerHour { get; set; }
        public double? AverageTweetsPerMinute { get; set; }
        public double? AverageTweetsPerSecond { get; set; }

        public SortedList<string, int>? TopDomains { get; set; }
        public SortedList<string, int>? TopEmojis { get; set; }
        public SortedList<string, int>? TopHashTags { get; set; }

        public override string ToString()
        {
            return $"TotalNumberOfTweets: {TotalNumberOfTweets}, PercentOfTweetsContainingEmojis: {PercentOfTweetsContainingEmojis}";
        }
    }
}