using System;
using System.Collections.Generic;
using System.Text;

namespace JackIsBack.NetCoreLibrary.Utility
{
    public static class SharedStrings
    {
        // Actors
        public static string MainActorPath {get; private set;} = "/user/TweetGeneratorActor/MainActor";
        public static string PercentOfTweetsContainingEmojisActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/PercentOfTweetsContainingEmojisActor";
        public static string PercentOfTweetsWithPhotoUrlActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/PercentOfTweetsWithPhotoUrlActor";
        public static string PercentOfTweetsWithUrlActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/PercentOfTweetsWithUrlActor";
        public static string TopDomainsActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/TopDomainsActor";
        public static string TopEmojisUsedActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/TopEmojisUsedActor";
        public static string TopHashTagsActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/TopHashTagsActor";
        public static string TotalNumberOfTweetsActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/TotalNumberOfTweetsActor";
        public static string TweetAverageActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/TweetAverageActor";
        public static string TweetGeneratorActorPath {get; private set;} = "/user/TweetGeneratorActor";
        public static string TweetStatisticsActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor";


        // Analyzer Actors
        ///user/TweetGeneratorActor/TweetStatisticsActor
        public static string PercentOfTweetsContainingEmojisAnalyzerActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/PercentOfTweetsContainingEmojisAnalyzerActor";
        public static string PercentOfTweetsWithPhotoUrlAnalyzerActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/PercentOfTweetsWithPhotoUrlAnalyzerActor";
        public static string PercentOfTweetsWithUrlAnalyzerActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/PercentOfTweetsWithUrlAnalyzerActor";
        public static string TopDomainsAnalyzerActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/TopDomainsAnalyzerActor";
        public static string TopEmojisUsedAnalyzerActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/TopEmojisUsedAnalyzerActor";
        public static string TopHashTagsAnalyzerActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/TopHashTagsAnalyzerActor";
        public static string TotalNumberOfTweetsAnalyzerActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/TotalNumberOfTweetsAnalyzerActor";
        public static string TweetAverageAnalyzerActorPath {get; private set;} = "/user/TweetGeneratorActor/TweetStatisticsActor/TweetAverageAnalyzerActor";

    }
}
