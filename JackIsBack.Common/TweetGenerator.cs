using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using JackIsBack.Common.Actors;
using JackIsBack.Common.Interfaces;
using System;
using System.Threading.Tasks;
using Akka.Routing;
using Akka.Util.Internal;
using Tweetinvi;
using Tweetinvi.Events;
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

            _container = builder.Build();
            _resolver = new AutoFacDependencyResolver(_container, _actorSystem);
        }

        private void InitializeActorSystemAnActors()
        {
            // Init TotalNumberOfTweetsActor
            //var totalNumberOfTweetsActorProps = _actorSystem.DI().Props(typeof(TotalNumberOfTweetsActor));
            var totalNumberOfTweetsActorProps = Props.Create<TotalNumberOfTweetsActor>().WithRouter(FromConfig.Instance);
            _totalNumberOfTweetsActorRef =
                _actorSystem.ActorOf(totalNumberOfTweetsActorProps, "TotalNumberOfTweetsActor");

            // Init TweetStatisticsActor
            var tweetStatisticsActorProps = Props.Create<TweetStatisticsActor>().WithRouter(FromConfig.Instance);
            _tweetStatisticsActorRef =
                _actorSystem.ActorOf(tweetStatisticsActorProps, "TweetStatisticsActor");

            ///-----------------------------

            // Init TweetAverageActor
            var tweetAverageActorProps = _actorSystem.DI().Props(typeof(TweetAverageActor));
            _tweetAverageActorRef = _actorSystem.ActorOf(tweetAverageActorProps, "TweetAverageActor");

            // Init TopEmojisUsedActor
            var topEmojisUsedActorProps = _actorSystem.DI().Props(typeof(TopEmojisUsedActor));
            _topEmojisUsedActorRef =
                _actorSystem.ActorOf(topEmojisUsedActorProps, "TopEmojisUsedActor");

            // Init PercentOfTweetsContainingEmojisActor
            var percentOfTweetsContainingEmojisActorProps = _actorSystem.DI().Props(typeof(PercentOfTweetsContainingEmojisActor));
            _percentOfTweetsContainingEmojisActorRef =
                _actorSystem.ActorOf(percentOfTweetsContainingEmojisActorProps, "PercentOfTweetsContainingEmojisActor");

            // Init TopHashTagsActor
            var topHashTagsActorProps = _actorSystem.DI().Props(typeof(TopHashTagsActor));
            _topHashTagsActorRef =
                _actorSystem.ActorOf(topHashTagsActorProps, "TopHashTagsActor");

            // Init PercentOfTweetsWithUrlActor
            var percentOfTweetsWithUrlActorProps = _actorSystem.DI().Props(typeof(PercentOfTweetsWithUrlActor));
            _percentOfTweetsWithUrlActorRef =
                _actorSystem.ActorOf(percentOfTweetsWithUrlActorProps, "PercentOfTweetsWithUrlActor");

            // Init PercentOfTweetsWithPhotoUrlActor
            var percentOfTweetsWithPhotoUrlActorProps = _actorSystem.DI().Props(typeof(PercentOfTweetsWithPhotoUrlActor));
            _percentOfTweetsWithPhotoUrlActorRef =
                _actorSystem.ActorOf(percentOfTweetsWithPhotoUrlActorProps, "PercentOfTweetsWithPhotoUrlActor");

            // Init TopDomainsActor
            var topDomainsActorProps = _actorSystem.DI().Props(typeof(TopDomainsActor));
            _topDomainsActorRef =
                _actorSystem.ActorOf(topDomainsActorProps, "TopDomainsActor");


        }


        //_actorSystem.ActorSelection("").Anchor
        private void InitializeActorSystemAnActors2()
        {
            var actorPathBase = "akka://TwitterStatisticsActorSystem/user/";
            // Init TotalNumberOfTweetsActor
            _totalNumberOfTweetsActorRef = _actorSystem.ActorSelection(actorPathBase + "TotalNumberOfTweetsActor").Anchor;

            // Init TweetAverageActor
            _tweetAverageActorRef = _actorSystem.ActorSelection(actorPathBase + "TweetAverageActor").Anchor;

            // Init TopEmojisUsedActor
            _topEmojisUsedActorRef = _actorSystem.ActorSelection(actorPathBase + "TopEmojisUsedActor").Anchor;

            // Init PercentOfTweetsContainingEmojisActor
            _percentOfTweetsContainingEmojisActorRef = _actorSystem.ActorSelection(actorPathBase + "PercentOfTweetsContainingEmojisActor").Anchor;

            // Init TopHashTagsActor
            _topHashTagsActorRef = _actorSystem.ActorSelection(actorPathBase + "TopHashTagsActor").Anchor;

            // Init PercentOfTweetsWithUrlActor
            _percentOfTweetsWithUrlActorRef = _actorSystem.ActorSelection(actorPathBase + "PercentOfTweetsWithUrlActor").Anchor;

            // Init PercentOfTweetsWithPhotoUrlActor
            _percentOfTweetsWithPhotoUrlActorRef = _actorSystem.ActorSelection(actorPathBase + "PercentOfTweetsWithPhotoUrlActor").Anchor;

            // Init TopDomainsActor
            _topDomainsActorRef = _actorSystem.ActorSelection(actorPathBase + "TopDomainsActor").Anchor;

            // Init TweetStatisticsActor
            _tweetStatisticsActorRef = _actorSystem.ActorSelection(actorPathBase + "TweetStatisticsActor").Anchor;
        }


        public TweetGenerator()
        {
            InitializeDIContainer();
            InitializeActorSystemAnActors();
        }

        public void SampleStreamOnTweetReceived(object? sender, TweetReceivedEventArgs e)
        {
            //TweetReceived?.Invoke(this, e);

            _totalNumberOfTweetsActorRef.Tell(e.Tweet);
            //_tweetAverageActorRef.Tell(e.Tweet);
            //_topEmojisUsedActorRef.Tell(e.Tweet);
            //_percentOfTweetsContainingEmojisActorRef.Tell(e.Tweet);
            //_topHashTagsActorRef.Tell(e.Tweet);
            //_percentOfTweetsWithUrlActorRef.Tell(e.Tweet);
            //_percentOfTweetsWithPhotoUrlActorRef.Tell(e.Tweet);
            //_topDomainsActorRef.Tell(e.Tweet);
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
        }
    }
}