namespace JackIsBack.NetCoreLibrary.Messages
{
    public class GetTotalNumberOfTweetsMessage
    {
        public GetTotalNumberOfTweetsMessage(int totalNumberOfTweets)
        {
            TotalNumberOfTweets = totalNumberOfTweets;
        }
        public int TotalNumberOfTweets { get; private set; }

        public override string ToString()
        {
            return $"TotalNumberOfTweets: {TotalNumberOfTweets}";
        }
    }
}