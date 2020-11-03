using System.Threading.Tasks;

namespace JackIsBack.NetCoreLibrary.Interfaces
{
    public interface ITweetGenerator
    {
        void Run();
        void Stop();
    }
}
