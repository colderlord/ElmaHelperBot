using System.Collections.Generic;
using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;
using RestSharp;

namespace EleWise.ELMA.ElmaBot.Core.Rest
{
    public interface IRestClient
    {
        Task<T> Execute<T>(string url, object requestData, Method method, Dictionary<string, string> additionalHeaders = null) where T : new();

        Task<T> ExecuteWithAuth<T>(string url, object requestData, Method method, Auth auth, Dictionary<string, string> additionalHeaders = null) where T : new();
    }
}
