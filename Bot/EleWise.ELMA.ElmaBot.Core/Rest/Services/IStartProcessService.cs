using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;

namespace EleWise.ELMA.ElmaBot.Core.Rest.Services
{
    public interface IStartProcessService
    {
        Task<StartProcessResult> StartProcessAsync(StartProcessBody body, Auth auth);

        Task<StartableProcesses> StartableProcesses(Auth auth);

        Task<StartProcessForm> StartProcessForm(long headerId, Auth auth);
    }
}
