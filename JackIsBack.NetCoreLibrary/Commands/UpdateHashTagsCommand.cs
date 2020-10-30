using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Interfaces;
using Tweetinvi.Core.Extensions;

namespace JackIsBack.NetCoreLibrary.Commands
{
    public class UpdateHashTagsCommand : ICommand
    {
        private readonly MyTweetDTO _tweet;
        private readonly List<KeyValuePair<string, long>> _values;
        public UpdateHashTagsCommand(MyTweetDTO tweet)
        {
            _tweet = tweet;
            _values = new List<KeyValuePair<string, long>>(); 
        }

        public void Execute()
        {
            var regex = new Regex(@"#\w+");
            var matches = regex.Matches(_tweet.Tweet);
            if (matches.Any())
            {
                long aValue;
                foreach (var match in matches)
                {
                    var key = match.ToString();
                    if (TweetStatistics.HashTags.ContainsKey(key))
                    {
                        if (TweetStatistics.HashTags.TryGetValue(key, out aValue))
                        {
                            TweetStatistics.HashTags.AddOrUpdate(key, aValue + 1);
                            _values.Add(new KeyValuePair<string, long>(key, aValue + 1));
                        }
                    }
                    else
                    {
                        TweetStatistics.HashTags.TryAdd(key, 1);
                        _values.Add(new KeyValuePair<string, long>(key, 1));
                    }
                }
            }
        }

        public bool CanExecute()
        {
            return true;
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, long> keyValuePair in _values)
            {
                sb.Append(keyValuePair.Key + " and " + keyValuePair.Value + " are present now.\n");
            }

            return sb.ToString();
        }
    }
}