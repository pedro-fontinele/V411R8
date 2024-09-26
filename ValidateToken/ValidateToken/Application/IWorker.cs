using System.Threading.Tasks;

namespace ValidateToken.Application
{
    public interface IWorker
    {
        Task RunAsync();
    }
}
