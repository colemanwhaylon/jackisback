using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Akka.Event;
using Akka.Routing;
using Akka.Util.Internal;
using Autofac;
using AutoMapper;
using JackIsBack.NetCoreLibrary.Actors.Analyzers;
using JackIsBack.NetCoreLibrary.Interfaces;
using JackIsBack.NetCoreLibrary.Messages;
using Serilog;
using System;
using JackIsBack.NetCoreLibrary.Utility;
using Tweetinvi;
using Tweetinvi.Core.DTO;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Streaming;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class TweetGeneratorActor : ReceiveActor, ITweetGenerator
    {
        private ILoggingAdapter _logger;
        private static ActorSystem ActorSystem;
        private ISampleStream _sampleStream;
        private static IContainer _container;
        private static IDependencyResolver _resolver;
        private static IActorRef _mainActorRef;
        private static IActorRef _tweetStatisticsActorRef;
        private bool _isInitialized = false;

        public TweetGeneratorActor()
        {
            _isInitialized = Initialize();

            // Init MainActor
            //_mainActorRef = ActorSystem.ActorOf(Props.Create<MainActor>().WithRouter(FromConfig.Instance), "MainActor");
            _mainActorRef = Context.System.ActorOf(Props.Create<MainActor>().WithRouter(FromConfig.Instance), "MainActor");
            _tweetStatisticsActorRef = Context.ActorOf(Props.Create<TweetStatisticsActor>(), "TweetStatisticsActor");
            _tweetStatisticsActorRef.Tell(new TimeKeeperActorMessage(DateTime.Now.Ticks, null));

            Receive<TweetGeneratorActorCommand>(HandleTweetGeneratorActorCommand);
            Receive<ChangeTotalNumberOfTweetsMessage>(HandleChangeTotalNumberOfTweetsMessage);
        }

        private void HandleChangeTotalNumberOfTweetsMessage(ChangeTotalNumberOfTweetsMessage message)
        {
            _logger.Debug($"Total Tweet NewTotal = {message.NewTotal}");
        }

        private void HandleTweetGeneratorActorCommand(TweetGeneratorActorCommand command)
        {
            if (command == TweetGeneratorActorCommand.StartUp)
                this.Run();
            else if (command == TweetGeneratorActorCommand.Shutdown)
            {
                this.Stop();
                this._isInitialized = false;
            }
        }

        private bool Initialize()
        {
            try
            {
                InitializeDIContainer();
            }
            catch (Exception exception)
            {
                _logger.Debug($"Message: {exception.Message}, StackTrace: {exception.StackTrace}");
            }

            return true;
        }


        private void InitializeDIContainer()
        {
            var logger = new LoggerConfiguration()
                //.WriteTo.Seq("http://localhost:5341") //todo:enable Seq before release to prod.
                .WriteTo.ColoredConsole()
                .WriteTo.RollingFile("log.txt",
                    Serilog.Events.LogEventLevel.Verbose,
                    fileSizeLimitBytes: 2048,
                    retainedFileCountLimit: 1,
                    buffered: true)
                .MinimumLevel.Verbose()
                .CreateLogger();
            Serilog.Log.Logger = logger;

            var builder = new ContainerBuilder();

            builder.RegisterType<TwitterInfo>().As<ITwitterInfo>();

            var twitterInfo = new TwitterInfo();
            var twitterClient = new TwitterClient(twitterInfo.Secrets.Key, twitterInfo.Secrets.SecretKey,
                twitterInfo.Secrets.AccessToken, twitterInfo.Secrets.AccessTokenSecret);

            builder.RegisterInstance(twitterClient).As<TwitterClient>();

            Serilog.Log.Logger.Information("TwitterStatisticsActorSystem created");

            builder.RegisterType<TweetGeneratorActor>().As<ITweetGenerator>();

            //Register All Actors
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
            builder.RegisterType<TimeKeeperActor>();

            //Register Analyzer Actors
            builder.RegisterType<PercentOfTweetsContainingEmojisAnalyzerActor>();
            builder.RegisterType<PercentOfTweetsWithPhotoUrlAnalyzerActor>();
            builder.RegisterType<PercentOfTweetsWithUrlAnalyzerActor>();
            builder.RegisterType<TopDomainsAnalyzerActor>();
            builder.RegisterType<TopEmojisUsedAnalyzerActor>();
            builder.RegisterType<TopHashTagsAnalyzerActor>();
            builder.RegisterType<TweetAverageAnalyzerActor>();

            //Register Messages
            builder.RegisterType<ChangeTotalNumberOfTweetsMessage>();
            builder.RegisterType<TimeKeeperActorMessage>();

            //AutoMapper Mapping types:
            builder.RegisterType<TweetDTO>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IUser, IUserDTO>();
                cfg.CreateMap<ITweet, TweetDTO>();
            });
            var mapper = config.CreateMapper();
            builder.RegisterInstance<IMapper>(mapper);

            _container = builder.Build();
            _resolver = new AutoFacDependencyResolver(_container, Context.System);
            ActorSystem = Context.System;
            _logger = Context.GetLogger();
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

        private async void SampleStreamOnTweetReceived(object? sender, TweetReceivedEventArgs e)
        {
            //Making the executive decision not to process & count tweets with no text command.
            if (!string.IsNullOrEmpty(e.Tweet.Text))
            {
                var newTweet = new ChangeTotalNumberOfTweetsMessage(Operation.Increase, 1, true);
                //ActorSystem.ActorSelection(SharedStrings.TotalNumberOfTweetsActorPath).Tell(newTweet);

                var result = await ActorSystem.ActorSelection(SharedStrings.TotalNumberOfTweetsActorPath).Ask(newTweet);

                _logger.Debug($"New Total: {result}");
                //_mainActorRef.Tell(e.Tweet.Text);
            }
        }

        public void Stop()
        {
            _logger.Debug("Stop was called.");

            _sampleStream.Stop();
            _sampleStream.TweetReceived -= null;

            _logger.Debug("Stop Finished.");

            _tweetStatisticsActorRef.Tell(new TimeKeeperActorMessage(null, DateTime.Now.Ticks));
        }
    }
}