using System;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Akka.Routing;
using Akka.Util.Internal;
using Autofac;
using AutoMapper;
using JackIsBack.Common.Actors;
using JackIsBack.Common.Commands;
using JackIsBack.Common.DTO;
using JackIsBack.Common.Interfaces;
using Serilog;
using Tweetinvi;
using Tweetinvi.Core.DTO;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Streaming;

namespace JackIsBack.Common
{
    public class TweetGenerator : ITweetGenerator
    {
        private static ActorSystem _actorSystem;
        private ISampleStream _sampleStream;
        private static IContainer _container;
        private static IDependencyResolver _resolver;
        private static IActorRef _totalNumberOfTweetsActorRef;
        private static IActorRef _tweetAverageActorRef;
        private static IActorRef _topEmojisUsedActorRef;
        private static IActorRef _percentOfTweetsContainingEmojisActorRef;
        private static IActorRef _topHashTagsActorRef;
        private static IActorRef _percentOfTweetsWithUrlActorRef;
        private static IActorRef _percentOfTweetsWithPhotoUrlActorRef;
        private static IActorRef _topDomainsActorRef;
        private static IActorRef _tweetStatisticsActorRef;
        public event EventHandler<TweetReceivedEventArgs> TweetReceived;

        private void InitializeDIContainer()
        {
            var logger = new LoggerConfiguration()
                //.WriteTo.Seq("http://localhost:5341") //todo:enable Seq before release to prod.
                .WriteTo.ColoredConsole()
                .WriteTo.RollingFile("log.txt",
                    Serilog.Events.LogEventLevel.Information,
                    fileSizeLimitBytes: 1024,
                    retainedFileCountLimit: 1,
                    buffered: false)
                .MinimumLevel.Information()
                .CreateLogger();
            Serilog.Log.Logger = logger;

            var builder = new ContainerBuilder();

            builder.RegisterType<TwitterInfo>().As<ITwitterInfo>();

            var twitterInfo = new TwitterInfo();
            var twitterClient = new TwitterClient(twitterInfo.Secrets.Key, twitterInfo.Secrets.SecretKey,
                twitterInfo.Secrets.AccessToken, twitterInfo.Secrets.AccessTokenSecret);

            builder.RegisterInstance(twitterClient).As<TwitterClient>();

            _actorSystem = ActorSystem.Create("TwitterStatisticsActorSystem");
            System.Console.WriteLine("TwitterStatisticsActorSystem created");

            builder.RegisterType<TweetGenerator>()
                .As<ITweetGenerator>();

            builder.RegisterType<TotalNumberOfTweetsActor>();
            builder.RegisterType<TweetAverageActor>();
            builder.RegisterType<TopEmojisUsedActor>();
            builder.RegisterType<PercentOfTweetsContainingEmojisActor>();
            builder.RegisterType<TopHashTagsActor>();
            builder.RegisterType<PercentOfTweetsWithUrlActor>();
            builder.RegisterType<PercentOfTweetsWithPhotoUrlActor>();
            builder.RegisterType<TopDomainsActor>();
            builder.RegisterType<TweetStatisticsActor>();
            builder.RegisterType<TweetDTO>();
            builder.RegisterType<MyTweetDTO>().As<IMyTweetDTO>();
            ///* Mapping types:

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IUser, IUserDTO>();
                cfg.CreateMap<ITweet, TweetDTO>();
            });
            var mapper = config.CreateMapper();
            builder.RegisterInstance<IMapper>(mapper);

            _container = builder.Build();
            _resolver = new AutoFacDependencyResolver(_container, _actorSystem);
        }

        private void InitializeActorSystemAnActors()
        {
            // Init TotalNumberOfTweetsActor
            var totalNumberOfTweetsActorProps = _actorSystem.DI().Props<TotalNumberOfTweetsActor>().WithRouter(FromConfig.Instance);
            _totalNumberOfTweetsActorRef =
                _actorSystem.ActorOf(totalNumberOfTweetsActorProps, "TotalNumberOfTweetsActor");

            // Init TweetStatisticsActor
            var tweetStatisticsActorProps = _actorSystem.DI().Props<TweetStatisticsActor>().WithRouter(FromConfig.Instance);
            _tweetStatisticsActorRef =
                _actorSystem.ActorOf(tweetStatisticsActorProps, "TweetStatisticsActor");

            // Init TweetAverageActor
            var tweetAverageActorProps = _actorSystem.DI().Props<TweetAverageActor>().WithRouter(FromConfig.Instance);
            _tweetAverageActorRef = _actorSystem.ActorOf(tweetAverageActorProps, "TweetAverageActor");

            // Init TopEmojisUsedActor
            var topEmojisUsedActorProps = _actorSystem.DI().Props<TopEmojisUsedActor>().WithRouter(FromConfig.Instance);
            _topEmojisUsedActorRef =
                _actorSystem.ActorOf(topEmojisUsedActorProps, "TopEmojisUsedActor");

            // Init PercentOfTweetsContainingEmojisActor
            var percentOfTweetsContainingEmojisActorProps =
                _actorSystem.DI().Props<PercentOfTweetsContainingEmojisActor>().WithRouter(FromConfig.Instance);
            _percentOfTweetsContainingEmojisActorRef =
                _actorSystem.ActorOf(percentOfTweetsContainingEmojisActorProps, "PercentOfTweetsContainingEmojisActor");

            // Init TopHashTagsActor
            var topHashTagsActorProps = _actorSystem.DI().Props<TopHashTagsActor>().WithRouter(FromConfig.Instance);
            _topHashTagsActorRef =
                _actorSystem.ActorOf(topHashTagsActorProps, "TopHashTagsActor");

            // Init PercentOfTweetsWithUrlActor
            var percentOfTweetsWithUrlActorProps = _actorSystem.DI().Props<PercentOfTweetsWithUrlActor>().WithRouter(FromConfig.Instance);
            _percentOfTweetsWithUrlActorRef =
                _actorSystem.ActorOf(percentOfTweetsWithUrlActorProps, "PercentOfTweetsWithUrlActor");

            // Init PercentOfTweetsWithPhotoUrlActor
            var percentOfTweetsWithPhotoUrlActorProps = _actorSystem.DI().Props<PercentOfTweetsWithPhotoUrlActor>().WithRouter(FromConfig.Instance);
            _percentOfTweetsWithPhotoUrlActorRef =
                _actorSystem.ActorOf(percentOfTweetsWithPhotoUrlActorProps, "PercentOfTweetsWithPhotoUrlActor");

            // Init TopDomainsActor
            var topDomainsActorProps = _actorSystem.DI().Props<TopDomainsActor>().WithRouter(FromConfig.Instance);
            _topDomainsActorRef =
                _actorSystem.ActorOf(topDomainsActorProps, "TopDomainsActor");
        }

        public TweetGenerator()
        {
            TweetStatistics.StartDateTime = new TimeSpan(DateTime.Now.Ticks);

            InitializeHourlyTweetCountDict();
            InitializeTweetStatisticsAverageCounters();
            InitializeDIContainer();
            InitializeActorSystemAnActors();
        }

        private void InitializeTweetStatisticsAverageCounters()
        {
            TweetStatistics.AverageTweetsPerHour = 0;
            TweetStatistics.AverageTweetsPerMinute = 0;
            TweetStatistics.AverageTweetsPerSecond = 0;
        }

        private void InitializeHourlyTweetCountDict()
        {
            TweetStatistics.HourlyTweetCountDict = new System.Collections.Generic.Dictionary<int, TweetInstancePerHour>();
            var hourIncrement = TweetStatistics.StartDateTime.Hours;
            for (var i = 0; i < 24; i++)
            {
                System.Console.WriteLine($"[Before Loop] i={i}, hourIncrement={hourIncrement}");

                var tiph = new TweetInstancePerHour
                {
                    Hour = hourIncrement >= 23 ? hourIncrement = 0 : ++hourIncrement,
                    Minute = TweetStatistics.StartDateTime.Minutes,
                    Second = TweetStatistics.StartDateTime.Seconds,
                    MilliSecond = TweetStatistics.StartDateTime.Milliseconds,
                    TotalTweetCountPerHour = 0
                };

                System.Console.WriteLine($"[After Loop] i={i}, hourIncrement={hourIncrement}");

                TweetStatistics.HourlyTweetCountDict.Add(i, tiph);
            }
        }

        public void SampleStreamOnTweetReceived(object? sender, TweetReceivedEventArgs e)
        {
            IMyTweetDTO myTweetDTO = null;
            try
            {
                var mapper = _container.Resolve<IMapper>();
                TweetDTO tweetDto = mapper.Map<TweetDTO>(e.Tweet);
                myTweetDTO = new MyTweetDTO
                {
                    Tweet = tweetDto
                };
            }
            catch (Exception ex)
            {

            }

            _totalNumberOfTweetsActorRef.Tell(myTweetDTO);
            _tweetAverageActorRef.Tell(myTweetDTO);
            //_topEmojisUsedActorRef.Tell(myTweetDTO);
            //_percentOfTweetsContainingEmojisActorRef.Tell(myTweetDTO);
            //_topHashTagsActorRef.Tell(myTweetDTO);
            //_percentOfTweetsWithUrlActorRef.Tell(myTweetDTO);
            //_percentOfTweetsWithPhotoUrlActorRef.Tell(myTweetDTO;
            //_topDomainsActorRef.Tell(myTweetDTO);
        }

        public async Task Run()
        {
            System.Console.WriteLine("Run was called.");

            var twitterClient = _container.Resolve(typeof(TwitterClient)).AsInstanceOf<TwitterClient>();
            _sampleStream = twitterClient.Streams.CreateSampleStream();
            _sampleStream.TweetReceived += SampleStreamOnTweetReceived;
            _sampleStream.StartAsync();

            System.Console.WriteLine("Run Finished.");
        }

        public async Task Stop()
        {
            System.Console.WriteLine("Stop was called.");

            _sampleStream.Stop();
            _sampleStream.TweetReceived -= null;

            System.Console.WriteLine("Stop Finished.");

            TweetStatistics.EndDateTime = new TimeSpan(DateTime.Now.Ticks);
        }
    }
}