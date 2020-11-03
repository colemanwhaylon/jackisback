using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Akka.Event;
using Akka.Util.Internal;
using Autofac;
using AutoMapper;
using JackIsBack.NetCoreLibrary.Actors;
using JackIsBack.NetCoreLibrary.Actors.Analyzers;
using JackIsBack.NetCoreLibrary.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Core.DTO;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Streaming;

namespace JackIsBack.NetCoreLibrary
{
    public class TweetGenerator : ReceiveActor, ITweetGenerator
    {
        private ILoggingAdapter _logger;
        private static ActorSystem _actorSystem;
        private ISampleStream _sampleStream;
        private static IContainer _container;
        private static IDependencyResolver _resolver;
        private static IActorRef _mainActorRef;
        private static IActorRef _tweetStatisticsActorRef;
        private bool _isInitialized = false;

        public TweetGenerator()
        {
            Receive<string>(HandleReceivedTweet);
        }

        private void HandleReceivedTweet(string message)
        {
            if (!_isInitialized)
            {
                this.Initialize();
                this._isInitialized = true;
            }

            if (message == "Run")
                this.Run();
            else if (message == "Stop")
            {
                this.Stop();
                this._isInitialized = false;
            }

            if (message != "Run" && message != "Stop")
                _mainActorRef.Tell(message);
        }
        public void Initialize()
        {
            TweetStatistics.StartDateTime = new TimeSpan(DateTime.Now.Ticks);

            InitializeTweetStatisticsAverageCounters();
            InitializeDIContainer();
            InitializeActorSystemAnActors();
            _logger = Context.GetLogger();

        }

        private void InitializeTweetStatisticsAverageCounters()
        {
            TweetStatistics.TotalTweetCount = 0;
            TweetStatistics.AverageTweetsPerHour = 0;
            TweetStatistics.AverageTweetsPerMinute = 0;
            TweetStatistics.AverageTweetsPerSecond = 0;
            TweetStatistics.HashTags = new Dictionary<string, long>();
        }

        private void InitializeDIContainer()
        {
            var logger = new LoggerConfiguration()
                //.WriteTo.Seq("http://localhost:5341") //todo:enable Seq before release to prod.
                .WriteTo.ColoredConsole()
                .WriteTo.RollingFile("log.txt",
                    Serilog.Events.LogEventLevel.Debug,
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

            Serilog.Log.Logger.Information("TwitterStatisticsActorSystem created");

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
            builder.RegisterType<MainActor>();
            builder.RegisterType<HashTagAnalyzerActor>();
            builder.RegisterType<TweetDTO>();

            ///* Mapping types:
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IUser, IUserDTO>();
                cfg.CreateMap<ITweet, TweetDTO>();
            });
            var mapper = config.CreateMapper();
            builder.RegisterInstance<IMapper>(mapper);

            _container = builder.Build();
            _resolver = new AutoFacDependencyResolver(_container, Context.System);
        }

        private void InitializeActorSystemAnActors()
        {
            // Init MainActor
            _mainActorRef = Context.ActorOf(Context.DI().Props<MainActor>(), "MainActor");

            _tweetStatisticsActorRef = Context.ActorOf(Context.DI().Props<TweetStatisticsActor>(), "TweetStatisticsActor");
            //var tweetStatisticsActorProps = Context.ActorOf(Context.DI()
            //    .Props<TweetStatisticsActor>());

            // Init TotalNumberOfTweetsActor
            //var totalNumberOfTweetsActorProps = _actorSystem.ActorOf(_actorSystem.DI()
            //    .Props<TotalNumberOfTweetsActor>()
            //    .WithRouter(FromConfig.Instance), "TotalNumberOfTweetsActor");

            //// Init TweetStatisticsActor
            //var tweetStatisticsActorProps = Context.ActorOf(Context.DI()
            //    .Props<TweetStatisticsActor>());

            //// Init TweetAverageActor
            //var tweetAverageActorProps = _actorSystem.DI()
            //    .Props<TweetAverageActor>()
            //    .WithRouter(new RoundRobinPool(40));
            //_tweetAverageActorRef = _actorSystem.ActorOf(tweetAverageActorProps, "TweetAverageActor");

            ////Init TopEmojisUsedActor
            //var topEmojisUsedActorProps = _actorSystem.DI().Props<TopEmojisUsedActor>().WithRouter(FromConfig.Instance);
            //_topEmojisUsedActorRef =
            //    _actorSystem.ActorOf(topEmojisUsedActorProps, "TopEmojisUsedActor");

            //// Init PercentOfTweetsContainingEmojisActor
            //var percentOfTweetsContainingEmojisActorProps =
            //    _actorSystem.DI().Props<PercentOfTweetsContainingEmojisActor>().WithRouter(FromConfig.Instance);
            //_percentOfTweetsContainingEmojisActorRef =
            //    _actorSystem.ActorOf(percentOfTweetsContainingEmojisActorProps, "PercentOfTweetsContainingEmojisActor");

            //// Init TopHashTagsActor
            ////var topHashTagsActorProps = _actorSystem.DI().Props<TopHashTagsActor>().WithRouter(FromConfig.Instance);
            //var topHashTagsActorProps = _actorSystem.DI().Props<TopHashTagsActor>().WithRouter(new RoundRobinPool(3));
            //_topHashTagsActorRef =
            //    _actorSystem.ActorOf(topHashTagsActorProps, "TopHashTagsActor");

            //// Init PercentOfTweetsWithUrlActor
            //var percentOfTweetsWithUrlActorProps = _actorSystem.DI().Props<PercentOfTweetsWithUrlActor>().WithRouter(FromConfig.Instance);
            //_percentOfTweetsWithUrlActorRef =
            //    _actorSystem.ActorOf(percentOfTweetsWithUrlActorProps, "PercentOfTweetsWithUrlActor");

            //// Init PercentOfTweetsWithPhotoUrlActor
            //var percentOfTweetsWithPhotoUrlActorProps = _actorSystem.DI().Props<PercentOfTweetsWithPhotoUrlActor>().WithRouter(FromConfig.Instance);
            //_percentOfTweetsWithPhotoUrlActorRef =
            //    _actorSystem.ActorOf(percentOfTweetsWithPhotoUrlActorProps, "PercentOfTweetsWithPhotoUrlActor");

            //// Init TopDomainsActor
            //var topDomainsActorProps = _actorSystem.DI().Props<TopDomainsActor>().WithRouter(FromConfig.Instance);
            //_topDomainsActorRef =
            //    _actorSystem.ActorOf(topDomainsActorProps, "TopDomainsActor");
        }

        public void Run()
        {
            _logger.Debug("Run was called.");

            var twitterClient = _container.Resolve(typeof(TwitterClient)).AsInstanceOf<TwitterClient>();
            _sampleStream = twitterClient.Streams.CreateSampleStream();
            _sampleStream.TweetReceived += SampleStreamOnTweetReceived;
            _sampleStream.StartAsync();

            _logger.Debug("Run Finished.");
        }

        private void SampleStreamOnTweetReceived(object? sender, TweetReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Tweet.Text))
                _mainActorRef.Tell(e.Tweet.Text);
        }

        public void Stop()
        {
            _logger.Debug("Stop was called.");

            _sampleStream.Stop();
            _sampleStream.TweetReceived -= null;

            _logger.Debug("Stop Finished.");

            TweetStatistics.EndDateTime = new TimeSpan(DateTime.Now.Ticks);
        }
    }
}