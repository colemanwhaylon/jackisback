using System;
using JackIsBack.NetCoreLibrary.Interfaces;

namespace JackIsBack.NetCoreLibrary.Commands
{
    public class UpdateTweetAverageCommand : ICommand
    {
        private long _amount;
        public UpdateTweetAverageCommand(long amount)
        {
            _amount = amount;
        }

        public bool CanExecute()
        {
            return TweetStatistics.TotalTweetCount >= 0;
        }

        public void Execute()
        {
            //var interval =  new TimeSpan(DateTime.Now.Subtract(TweetStatistics.StartDateTime).Ticks);
            //var totalTweets = _amount + TweetStatistics.TotalTweetCount;
            //var avgTweetsPerHour = totalTweets / interval.TotalHours;
            //var avgTweetsPerMinute = totalTweets / interval.TotalMinutes;
            //var avgTweetsPerSecond = totalTweets / interval.TotalSeconds;

            //TweetStatistics.AverageTweetsPerHour = (long)avgTweetsPerHour;
            //TweetStatistics.AverageTweetsPerMinute = (long)avgTweetsPerMinute;
            //TweetStatistics.AverageTweetsPerSecond = (long)avgTweetsPerSecond;
        }

        public void Undo()
        {

        }

        public override string ToString()
        {
            return $"Amount: {this._amount}";
        }
    }
}