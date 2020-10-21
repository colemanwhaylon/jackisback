using System;
using System.Net.Http;
using System.Threading.Tasks;
using Akka.Actor;
using JackIsBack.Console.Actors;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Tweetinvi;
using Tweetinvi.Core.DTO;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace JackIsBack.Console
{
    public class Driver
    {
        private HttpClient _httpClient;
        private TwitterInfo _twitterInfo;
        private TwitterClient _twitterClient;
        private static Driver _driver;

        private ISampleStream _sampleStream;
        private static ActorSystem TwitterStatisticsActorSystem;
        private IActorRef _totalNumberOfTweetsActorRef;
        private IActorRef _tweetAverageActorRef;
        private IActorRef _topEmojisUsedActorRef;
        private IActorRef _percentOfTweetsContainingEmojisActorRef;
        private IActorRef _topHashTagsActorRef;
        private IActorRef _percentOfTweetsWithUrlActorRef;
        private IActorRef _percentOfTweetsWithPhotoUrlActorRef;
        private IActorRef _topDomainsActorRef;

        static Driver()
        {
            _driver = new Driver();
        }

        public Driver()
        {
            Initialize();
        }

        private void Initialize()
        {
            InitializeDependencies();
            InitializeTweetInvi();
            InitializeActorSystemAnActors();
        }

        private void InitializeDependencies()
        {
            _twitterInfo = new TwitterInfo();
            _httpClient = new HttpClient();
        }

        private void InitializeTweetInvi()
        {
            _twitterClient = new TwitterClient(_twitterInfo.Secrets.Key,
                _twitterInfo.Secrets.SecretKey,
                _twitterInfo.Secrets.AccessToken,
                _twitterInfo.Secrets.AccessTokenSecret);
        }

        private void InitializeActorSystemAnActors()
        {
            TwitterStatisticsActorSystem = ActorSystem.Create("TwitterStatisticsActorSystem");
            System.Console.WriteLine("TwitterStatisticsActorSystem created");

            // Init TotalNumberOfTweetsActor
            var totalNumberOfTweetsActorProps = Props.Create<TotalNumberOfTweetsActor>();
            _totalNumberOfTweetsActorRef =
                TwitterStatisticsActorSystem.ActorOf(totalNumberOfTweetsActorProps, "TotalNumberOfTweetsActor");

            // Init TotalNumberOfTweetsActor
            var tweetAverageActorProps = Props.Create<TweetAverageActor>();
            _tweetAverageActorRef =
                TwitterStatisticsActorSystem.ActorOf(tweetAverageActorProps, "TweetAverageActor");

            // Init TotalNumberOfTweetsActor
            var topEmojisUsedActorProps = Props.Create<TopEmojisUsedActor>();
            _topEmojisUsedActorRef =
                TwitterStatisticsActorSystem.ActorOf(topEmojisUsedActorProps, "TopEmojisUsedActor");

            // Init TotalNumberOfTweetsActor
            var percentOfTweetsContainingEmojisActorProps = Props.Create<PercentOfTweetsContainingEmojisActor>();
            _percentOfTweetsContainingEmojisActorRef =
                TwitterStatisticsActorSystem.ActorOf(percentOfTweetsContainingEmojisActorProps, "PercentOfTweetsContainingEmojisActor");

            // Init TotalNumberOfTweetsActor
            var topHashTagsActorProps = Props.Create<TopHashTagsActor>();
            _topHashTagsActorRef =
                TwitterStatisticsActorSystem.ActorOf(topHashTagsActorProps, "TopHashTagsActor");

            // Init TotalNumberOfTweetsActor
            var percentOfTweetsWithUrlActorProps = Props.Create<PercentOfTweetsWithUrlActor>();
            _percentOfTweetsWithUrlActorRef =
                TwitterStatisticsActorSystem.ActorOf(percentOfTweetsWithUrlActorProps, "PercentOfTweetsWithUrlActor");

            // Init TotalNumberOfTweetsActor
            var percentOfTweetsWithPhotoUrlActorProps = Props.Create<PercentOfTweetsWithPhotoUrlActor>();
            _percentOfTweetsWithPhotoUrlActorRef =
                TwitterStatisticsActorSystem.ActorOf(percentOfTweetsWithPhotoUrlActorProps, "PercentOfTweetsWithPhotoUrlActor");

            // Init TotalNumberOfTweetsActor
            var topDomainsActorProps = Props.Create<TopDomainsActor>();
            _topDomainsActorRef =
                TwitterStatisticsActorSystem.ActorOf(topDomainsActorProps, "TopDomainsActor");
        }

        public static async Task Main(string[] args)
        {
            try
            {
                var twitterInfo = new TwitterInfo();
                var userClient = new TwitterClient(twitterInfo.Secrets.Key, twitterInfo.Secrets.SecretKey,
                    twitterInfo.Secrets.AccessToken, twitterInfo.Secrets.AccessTokenSecret);

                _driver._sampleStream = userClient.Streams.CreateSampleStream();
                _driver._sampleStream.TweetReceived += SampleStreamOnTweetReceived;

                await _driver._sampleStream.StartAsync();
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception);
            }
            finally
            {
                _driver._sampleStream.TweetReceived -= SampleStreamOnTweetReceived;
                _driver._sampleStream.TweetReceived -= null;

                _driver._sampleStream.Stop();
                await TwitterStatisticsActorSystem.Terminate();
            }

            System.Console.WriteLine("FINISHED!");
            System.Console.ReadLine();
        }

        private static void SampleStreamOnTweetReceived(object? sender, TweetReceivedEventArgs e)
        {
            _driver._totalNumberOfTweetsActorRef.Tell($"{e.Tweet.Text }");
        }

    }
}