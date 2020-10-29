using System.IO;
using JackIsBack.NetCoreLibrary.Interfaces;
using Microsoft.Extensions.Configuration;

namespace JackIsBack.NetCoreLibrary
{
    public class TwitterInfo : ITwitterInfo
    {
        public TwitterSecrets Secrets { get; set; }
        public TwitterApiLinks ApiLinks { get; set; }

        public TwitterInfo()
        {
            Secrets = new TwitterSecrets();
            ApiLinks = new TwitterApiLinks();
        }
    }

    public static class TwitterConfig
    {
        public static IConfigurationRoot GetConfigurationRoot()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configurationRoot = builder.Build();
            return configurationRoot;
        }
    }

    public class TwitterSecrets
    {
        public TwitterSecrets()
        {
            var icr = TwitterConfig.GetConfigurationRoot();
            Key = icr.GetSection("Twitter:Secrets:Key").Value;
            SecretKey = icr.GetSection("Twitter:Secrets:SecretKey").Value;
            BearerToken = icr.GetSection("Twitter:Secrets:BearerToken").Value;
            AccessToken = icr.GetSection("Twitter:Secrets:AccessToken").Value;
            AccessTokenSecret = icr.GetSection("Twitter:Secrets:AccessTokenSecret").Value;
        }

        public string Key { get; set; }
        public string SecretKey { get; set; }
        public string BearerToken { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }

    public class TwitterApiLinks
    {
        public TwitterApiLinks()
        {
            IConfigurationRoot icr = TwitterConfig.GetConfigurationRoot();

            MultipleTweets = icr.GetSection("Twitter:ApiLinks:MultipleTweets").Value;
            RecentSearch = icr.GetSection("Twitter:ApiLinks:RecentSearch").Value;
            SingleTweet = icr.GetSection("Twitter:ApiLinks:SingleTweet").Value;
            Stream = icr.GetSection("Twitter:ApiLinks:Stream").Value;
            UserById = icr.GetSection("Twitter:ApiLinks:UserById").Value;
            UserByUsername = icr.GetSection("Twitter:ApiLinks:UserByUsername").Value;
            UsersById = icr.GetSection("Twitter:ApiLinks:UsersById").Value;
            UsersByUsername = icr.GetSection("Twitter:ApiLinks:UsersByUsername").Value;
        }
        public string SingleTweet { get; set; }
        public string MultipleTweets { get; set; }
        public string UserById { get; set; }
        public string UsersById { get; set; }
        public string UserByUsername { get; set; }
        public string UsersByUsername { get; set; }
        public string RecentSearch { get; set; }
        public string Stream { get; set; }
    }
}