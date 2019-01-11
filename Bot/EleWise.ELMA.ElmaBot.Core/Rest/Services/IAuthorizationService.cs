using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;

namespace EleWise.ELMA.ElmaBot.Core.Rest.Services
{
    public interface IAuthorizationService
    {
        Task<Auth> LoginWithUserName(string userName, string password);
    }
}
