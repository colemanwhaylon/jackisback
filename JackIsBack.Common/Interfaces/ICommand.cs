using System.Text;

namespace JackIsBack.Common.Interfaces
{
    public enum Operation
    {
        Increase,
        Decrease
    }

    public interface ICommand
    {
        void Execute();
        bool CanExecute();
        void Undo();
    }
}
