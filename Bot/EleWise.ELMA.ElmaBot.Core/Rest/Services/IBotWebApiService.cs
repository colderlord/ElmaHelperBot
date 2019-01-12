using System.Collections.Generic;
using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;

namespace EleWise.ELMA.ElmaBot.Core.Rest.Services
{
    public interface IBotWebApiService
    {
        Task UpdateUser(string chatId, Auth auth);

        Task<List<ContextProcess>> GetProcessContext(long headerid);

        Task<bool> EventCreate(EventCreatorModel model);
    }

    public class BotWebApiService : IBotWebApiService
    {
        private readonly ELMARestClient restClient;

        public BotWebApiService(ELMARestClient restClient)
        {
            this.restClient = restClient;
        }

        public Task UpdateUser(string chatId, Auth auth)
        {
            return restClient.ExecuteWithAuth<StartProcessResult>($"/PublicAPI/REST/EleWise.ELMA.ELMAHelperBot/Bot/UpdateUser?chatId={chatId}&userId={auth.CurrentUserId}", null, RestSharp.Method.POST, auth);
        }

        public Task<List<ContextProcess>> GetProcessContext(long headerid)
        {
            return restClient.Execute<List<ContextProcess>>($"/PublicAPI/REST/EleWise.ELMA.ELMAHelperBot/Bot/GetProcessContext?headerid={headerid}", null, RestSharp.Method.GET);
        }

        public Task<bool> EventCreate(EventCreatorModel model)
        {
            return restClient.Execute<bool>("/PublicAPI/REST/EleWise.ELMA.ELMAHelperBot/Bot/EventCreate", model, RestSharp.Method.POST);
        }
    }

    
}
