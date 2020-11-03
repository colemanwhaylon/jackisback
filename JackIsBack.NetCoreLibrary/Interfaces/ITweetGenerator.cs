using System.Threading.Tasks;

namespace JackIsBack.NetCoreLibrary.Interfaces
{
    public interface ITweetGenerator
    {
        Task RunAsync();
        Task StopAsync();
    }
}
