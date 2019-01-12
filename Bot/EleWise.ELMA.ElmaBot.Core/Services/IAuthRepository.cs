using EleWise.ELMA.ElmaBot.Core.Rest.Models;

namespace EleWise.ELMA.ElmaBot.Core.Services
{
    public interface IAuthRepository
    {
        Auth GetCurrentAuth(long chatId);

        void SetAuth(long chatId, Auth auth);

        bool Authorized(long chatId);
    }
}
