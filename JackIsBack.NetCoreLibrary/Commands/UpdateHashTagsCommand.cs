using System;
using System.Text.RegularExpressions;
using JackIsBack.NetCoreLibrary.DTO;
using JackIsBack.NetCoreLibrary.Interfaces;

namespace JackIsBack.NetCoreLibrary.Commands
{
    public class UpdateHashTagsCommand : ICommand
    {
        private readonly MyTweetDTO _tweet;

        public UpdateHashTagsCommand(MyTweetDTO tweet)
        {
            _tweet = tweet;
        }

        public void Execute()
        {
            var regex = new Regex(@"#\w+");
            var matches = regex.Matches(_tweet.Tweet);
            foreach (var match in matches)
            {
                Console.WriteLine(match);


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
    }
}