using System.Text;

namespace JackIsBack.Common.Commands
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
