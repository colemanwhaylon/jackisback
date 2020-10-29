namespace JackIsBack.NetCoreLibrary.DTO
{
    public class MyTweetDTO
    {
        public MyTweetDTO(string tweet)
        {
            Tweet = tweet;
        }
        public string Tweet { get; private set; }
    }
}