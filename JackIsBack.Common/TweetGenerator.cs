using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using JackIsBack.Common.Actors;
using JackIsBack.Common.Interfaces;
using System;
using System.Threading.Tasks;
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

            _container = builder.Build();
            _resolver = new AutoFacDependencyResolver(_container, _actorSystem);
        }

        private void InitializeActorSystemAnActors()
        {
            // Init TotalNumberOfTweetsActor
            var totalNumberOfTweetsActorProps = _actorSystem.DI().Props(typeof(TotalNumberOfTweetsActor));
            _totalNumberOfTweetsActorRef =
                _actorSystem.ActorOf(totalNumberOfTweetsActorProps, "TotalNumberOfTweetsActor");

            // Init TotalNumberOfTweetsActor
            var tweetAverageActorProps = _actorSystem.DI().Props(typeof(TweetAverageActor));
            _tweetAverageActorRef = _actorSystem.ActorOf(tweetAverageActorProps, "TweetAverageActor");

            // Init TotalNumberOfTweetsActor
            var topEmojisUsedActorProps = _actorSystem.DI().Props(typeof(TopEmojisUsedActor));
            _topEmojisUsedActorRef =
                _actorSystem.ActorOf(topEmojisUsedActorProps, "TopEmojisUsedActor");

            // Init TotalNumberOfTweetsActor
            var percentOfTweetsContainingEmojisActorProps = _actorSystem.DI().Props(typeof(PercentOfTweetsContainingEmojisActor));
            _percentOfTweetsContainingEmojisActorRef =
                _actorSystem.ActorOf(percentOfTweetsContainingEmojisActorProps, "PercentOfTweetsContainingEmojisActor");

            // Init TotalNumberOfTweetsActor
            var topHashTagsActorProps = _actorSystem.DI().Props(typeof(TopHashTagsActor));
            _topHashTagsActorRef =
                _actorSystem.ActorOf(topHashTagsActorProps, "TopHashTagsActor");

            // Init TotalNumberOfTweetsActor
            var percentOfTweetsWithUrlActorProps = _actorSystem.DI().Props(typeof(PercentOfTweetsWithUrlActor));
            _percentOfTweetsWithUrlActorRef =
                _actorSystem.ActorOf(percentOfTweetsWithUrlActorProps, "PercentOfTweetsWithUrlActor");

            // Init TotalNumberOfTweetsActor
            var percentOfTweetsWithPhotoUrlActorProps = _actorSystem.DI().Props(typeof(PercentOfTweetsWithPhotoUrlActor));
            _percentOfTweetsWithPhotoUrlActorRef =
                _actorSystem.ActorOf(percentOfTweetsWithPhotoUrlActorProps, "PercentOfTweetsWithPhotoUrlActor");

            // Init TotalNumberOfTweetsActor
            var topDomainsActorProps = _actorSystem.DI().Props(typeof(TopDomainsActor));
            _topDomainsActorRef =
                _actorSystem.ActorOf(topDomainsActorProps, "TopDomainsActor");
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
            _tweetAverageActorRef.Tell(e.Tweet);
            _topEmojisUsedActorRef.Tell(e.Tweet);
            _percentOfTweetsContainingEmojisActorRef.Tell(e.Tweet);
            _topHashTagsActorRef.Tell(e.Tweet);
            _percentOfTweetsWithUrlActorRef.Tell(e.Tweet);
            _percentOfTweetsWithPhotoUrlActorRef.Tell(e.Tweet);
            _topDomainsActorRef.Tell(e.Tweet);
        }

        //The event-invoking method that derived classes can override.
        //protected virtual void OnShapeChanged(TweetReceivedEventArgs e)
        //{
        //    System.Console.WriteLine("In the TweetReceived method handler.");
        //    // Safely raise the event for all subscribers
        //    TweetReceived?.Invoke(this, e);
        //}

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