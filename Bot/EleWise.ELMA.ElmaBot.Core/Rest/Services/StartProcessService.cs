using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;

namespace EleWise.ELMA.ElmaBot.Core.Rest.Services
{
    public class StartProcessService : IStartProcessService
    {
        private readonly ELMARestClient restClient;

        public StartProcessService(ELMARestClient restClient)
        {
            this.restClient = restClient;
        }

        public Task<StartProcessResult> StartProcessAsync(StartProcessBody body, Auth auth)
        {
            return restClient.ExecuteWithAuth<StartProcessResult>($"/Workflow/StartProcessAsync", body, RestSharp.Method.POST, auth);
        }
    }
}
