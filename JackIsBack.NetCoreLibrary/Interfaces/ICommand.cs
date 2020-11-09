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

    public enum TweetGeneratorActorCommandResponse
    {
        None,
        StartedUp,
        ShutdownCompletely,
        ExceptionOccurred
    }

    public interface ICommand
    {
        void Execute();
        bool CanExecute();
        void Undo();
    }
}
