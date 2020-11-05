using System;
using System.Collections.Generic;
using System.Text;

namespace JackIsBack.NetCoreLibrary.Utility
{
    public static class SharedStrings
    {
        // Actors
        public static string MainActorPath {get; private set;} = "";
        public static string PercentOfTweetsContainingEmojisActorPath {get; private set;} = "";
        public static string PercentOfTweetsWithPhotoUrlActorPath {get; private set;} = "";
        public static string PercentOfTweetsWithUrlActorPath {get; private set;} = "";
        public static string TopDomainsActorPath {get; private set;} = "";
        public static string TopEmojisUsedActorPath {get; private set;} = "";
        public static string TopHashTagsActorPath {get; private set;} = "";
        public static string TotalNumberOfTweetsActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/TotalNumberOfTweetsActor";
        public static string TweetAverageActorPath {get; private set;} = "";
        public static string TweetGeneratorActorPath {get; private set;} = "";
        public static string TweetStatisticsActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor";


        // Analyzer Actors
        public static string PercentOfTweetsContainingEmojisAnalyzerActorPath {get; private set;} = "/user/PercentOfTweetsContainingEmojisAnalyzerActor";
        public static string PercentOfTweetsWithPhotoUrlAnalyzerActorPath {get; private set;} = "/user/PercentOfTweetsWithPhotoUrlAnalyzerActor";
        public static string PercentOfTweetsWithUrlAnalyzerActorPath {get; private set;} = "/user/PercentOfTweetsWithUrlAnalyzerActor";
        public static string TopDomainsAnalyzerActorPath {get; private set;} = "/user/TopDomainsAnalyzerActor";
        public static string TopEmojisUsedAnalyzerActorPath {get; private set;} = "/user/TopEmojisUsedAnalyzerActor";
        public static string TopHashTagsAnalyzerActorPath {get; private set;} = "/user/TopHashTagsAnalyzerActor";
        public static string TotalNumberOfTweetsAnalyzerActorPath {get; private set;} = "/user/TotalNumberOfTweetsAnalyzerActor";
        public static string TweetAverageAnalyzerActorPath {get; private set;} = "/user/TweetAverageAnalyzerActor";

    }
}
