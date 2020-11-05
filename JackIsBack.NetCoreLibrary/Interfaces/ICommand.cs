namespace JackIsBack.NetCoreLibrary.Interfaces
{
    public enum Operation
    {
        Increase,
        Decrease
    }

    public enum TweetGeneratorActorCommand
    {
        None,
        StartUp,
        Shutdown
    }

    public interface ICommand
    {
        void Execute();
        bool CanExecute();
        void Undo();
    }
}
