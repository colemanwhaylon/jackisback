using Tweetinvi.Models;

namespace JackIsBack.NetCoreLibrary.Messages
{
    public class TwitterMessage
    {

        public ITweet Tweet { get; private set; }

        public TwitterMessage(ITweet tweet)
        {
            Tweet = tweet;
        }
    }
}
