using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;

namespace EleWise.ELMA.ElmaBot.Core.Rest.Services
{
    public interface IBotWebApiService
    {
        Task UpdateUser(string chatId, Auth auth);
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
    }
}
