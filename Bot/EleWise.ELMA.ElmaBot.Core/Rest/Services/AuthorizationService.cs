using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;

namespace EleWise.ELMA.ElmaBot.Core.Rest.Services
{
    public sealed class AuthorizationService : IAuthorizationService
    {
        private readonly ELMARestClient restClient;

        public AuthorizationService(ELMARestClient restClient)
        {
            this.restClient = restClient;
        }

        public Task<Auth> LoginWithUserName(string userName, string password)
        {
            return restClient.Execute<Auth>($"/Authorization/LoginWith?username={userName}", password, RestSharp.Method.POST);
        }
    }
}
