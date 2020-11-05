using JackIsBack.NetCoreLibrary.Interfaces;

namespace JackIsBack.NetCoreLibrary.Messages
{
    public class ChangeTotalNumberOfTweetsMessage 
    {
        public Operation Operation { get; private set; }
        public int Total { get; private set; }


        public ChangeTotalNumberOfTweetsMessage(Operation operation, int amount)
        {
            Operation = operation;
            Total = amount;
        }
       
        public override string ToString()
        {
            return $"Operation: {this.Operation}, Amount: {this.Total}";
        }
    }
}