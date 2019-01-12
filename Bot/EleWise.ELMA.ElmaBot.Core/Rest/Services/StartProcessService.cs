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
            return restClient.ExecuteWithAuth<StartProcessResult>($"/api/rest/Workflow/StartProcessAsync", body, RestSharp.Method.POST, auth);
        }

        public Task<StartableProcesses> StartableProcesses(Auth auth)
        {
            return restClient.ExecuteWithAuth<StartableProcesses>($"/api/rest/Workflow/StartableProcesses", null, RestSharp.Method.POST, auth);
        }
    }
}
