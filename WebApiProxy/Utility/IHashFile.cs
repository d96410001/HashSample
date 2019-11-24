using System.Threading.Tasks;

namespace WebApiProxy.Utility
{
    public interface IHashFile
    {
        Task<string> GetHashFileBase64Async(string fileUri);

        Task<string> SimulatorHashFileBase64Async(string fileUri);
    }
}