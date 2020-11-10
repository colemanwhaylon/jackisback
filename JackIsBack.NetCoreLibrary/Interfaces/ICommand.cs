namespace JackIsBack.NetCoreLibrary.Interfaces
{
    public enum Operation
    {
        Increase,
        Decrease
    }

    public enum InitToggleCommandRequest
    {
        None,
        StartUp,
        Shutdown
    }

    public enum InitToggleCommandResponse
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
