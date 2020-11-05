using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Commands;
using System.Collections.Generic;

namespace JackIsBack.NetCoreLibrary.Actors
{
    public class TopHashTagsActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public TopHashTagsActor()
        {
            _logger.Debug("TopHashTagsActor created.");
            Receive<List<string>>(HandleTwitterMessageAsync);
        }

        private void HandleTwitterMessageAsync(List<string> hasTags)
        {
            var command = new UpdateListOfHashTagsCommand(hasTags);
            Context.ActorSelection("akka://TwitterStatisticsActorSystem/user/TweetStatisticsActor").Tell(command);
        }
    }
}
