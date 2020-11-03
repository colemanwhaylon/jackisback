using Akka.Actor;
using Akka.DI.Core;
using Akka.Event;
using Akka.Routing;
using JackIsBack.NetCoreLibrary.Actors.Analyzers;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class MainActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly IActorRef _hashTagAnalyzerActor;

        public MainActor()
        {
            _logger.Debug("MainActor created.");
            _hashTagAnalyzerActor = Context.ActorOf(Context.DI().Props<HashTagAnalyzerActor>().WithRouter(FromConfig.Instance));
        }
       
    }
}