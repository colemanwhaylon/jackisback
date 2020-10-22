using System;
using JackIsBack.Common.Actors;
using Tweetinvi.Models;

namespace JackIsBack.Common.Commands
{
    public class ChangeTweetQuantityCommand : ICommand
    {

        private readonly Operation _operation;
        private readonly int _amount;

        public ChangeTweetQuantityCommand(Operation operation, int amount)
        {
            _operation = operation;
            _amount = amount;
        }

        public void Execute()
        {
            switch (_operation)
            {
                case Operation.Increase:
                    TweetStatistics.Count += _amount;
                    break;
                case Operation.Decrease:
                    TweetStatistics.Count-= _amount;
                    break;
            }
        }

        public bool CanExecute()
        {
            return TweetStatistics.Count >= 0;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}