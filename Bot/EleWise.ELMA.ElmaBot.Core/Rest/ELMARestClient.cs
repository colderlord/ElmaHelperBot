using EleWise.ELMA.ElmaBot.Core.Rest.Models;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EleWise.ELMA.ElmaBot.Core.Rest
{
    /// <summary>
    /// Rest клиент
    /// </summary>
    public sealed class ELMARestClient : IRestClient
    {
        // Токен ELMAAgent
        private const string appToken = "285C8352AA7C67BFF882E4F236DECF51098C141AFB33A2AA4F7B34B4B3CEEF5DA30C848591DA55D5226C5D8D2C36432B12A5EF86C3D2EDF7E7C5781EC9D4E14A";
        private RestClient client;
        
        public ELMARestClient()
        {
            client = new RestClient("http://localhost:4747");
        }

        public Task<T> Execute<T>(string url, object requestData, Method method, Dictionary<string, string> additionalHeaders = null) where T : new()
        {
            return ExecuteWithAuth<T>(url, requestData, method, null, additionalHeaders);
        }

        public async Task<T> ExecuteWithAuth<T>(string url, object requestData, Method method, Auth auth, Dictionary<string, string> additionalHeaders = null) where T : new()
        {
            var request = new RestRequest(url, method);
            request.RequestFormat = DataFormat.Json;

            AddDefaultHeaders(request, auth);

            if (additionalHeaders != null)
            {
                foreach (var header in additionalHeaders)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            if (requestData != null)
            {
                request.AddJsonBody(requestData);
            }

            request.OnBeforeDeserialization = resp => 
            {
                // Нечитаемый первый символ https://jira4.elewise.com/browse/ELMA-20504
                resp.Content = resp.Content.TrimStart(resp.Content[0]);
            };

            var response = await client.ExecuteTaskAsync<T>(request);
            return response.Data;
        }

        private void AddDefaultHeaders(IRestRequest request, Auth auth)
        {
            //Добавляем токен приложения
            request.AddHeader("ApplicationToken", appToken);
            request.AddHeader("WebData-Version", "2.0");
            if (auth != null)
            {
                request.AddHeader("AuthToken", auth.AuthToken);
                request.AddHeader("SessionToken", auth.SessionToken);
            }
        }
    }
}
