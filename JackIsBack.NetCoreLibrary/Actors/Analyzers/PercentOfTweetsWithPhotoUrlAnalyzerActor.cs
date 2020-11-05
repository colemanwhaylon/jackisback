﻿using Akka.Actor;
using Akka.Event;
using JackIsBack.NetCoreLibrary.Utility;

namespace JackIsBack.NetCoreLibrary.Actors.Analyzers
{
    public class PercentOfTweetsWithPhotoUrlAnalyzerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public PercentOfTweetsWithPhotoUrlAnalyzerActor()
        {
            _logger.Debug("PercentOfTweetsWithPhotoUrlAnalyzerActor created.");

            Receive<string>(AnalyzeTwitterMessage);
        }

        private void AnalyzeTwitterMessage(string text)
        {
            _logger.Debug($"PercentOfTweetsWithPhotoUrlAnalyzerActor is analyzing tweet message: {text}");

            Context.ActorSelection(SharedStrings.TweetStatisticsActorPath).Tell(text);
            Context.Self.Tell(PoisonPill.Instance);
        }
    }
}