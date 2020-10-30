using System;
using JackIsBack.NetCoreLibrary.Interfaces;

namespace JackIsBack.NetCoreLibrary.Commands
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
                    TweetStatistics.TotalTweetCount += _amount;
                    break;
                case Operation.Decrease:
                    TweetStatistics.TotalTweetCount-= _amount;
                    break;
            }
        }

        public bool CanExecute()
        {
            return TweetStatistics.TotalTweetCount >= 0;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Operation: {this._operation}, Amount: {this._amount}";
        }
    }
}