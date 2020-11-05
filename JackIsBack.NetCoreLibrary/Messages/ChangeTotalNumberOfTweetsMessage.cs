using JackIsBack.NetCoreLibrary.Interfaces;

namespace JackIsBack.NetCoreLibrary.Messages
{
    public class ChangeTotalNumberOfTweetsMessage 
    {
        public Operation Operation { get; private set; }
        public int Total { get; private set; }
        public int NewTotal { get; private set; }
        public bool NeedsResponse { get; private set; }

        public ChangeTotalNumberOfTweetsMessage(Operation operation, int amount, bool needsResponse = false)
        {
            Operation = operation;
            Total = amount;
            NeedsResponse = needsResponse;
        }

        public ChangeTotalNumberOfTweetsMessage(Operation operation, int amount, int newTotal = 0, bool needsResponse = false)
        {
            Operation = operation;
            Total = amount;
            NewTotal = newTotal;
            NeedsResponse = needsResponse;
        }
       
        public override string ToString()
        {
            return $"Operation: {this.Operation}, Amount: {this.Total}, New Total: {this.NewTotal}, Needs Response: {this.NeedsResponse}";
        }
    }

}