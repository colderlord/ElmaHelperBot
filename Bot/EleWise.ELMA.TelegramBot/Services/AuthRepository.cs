using System.Collections.Generic;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;
using EleWise.ELMA.ElmaBot.Core.Services;

namespace EleWise.ELMA.TelegramBot.Services
{
    public class AuthRepository : IAuthRepository
    {
        private Dictionary<long, Auth> loginRepo;

        public AuthRepository()
        {
            loginRepo = new Dictionary<long, Auth>();
        }

        public Auth GetCurrentAuth(long chatId)
        {
            if (loginRepo.TryGetValue(chatId, out Auth auth))
            {
                return auth;
            }
            return null;
        }

        public void SetAuth(long chatId, Auth auth)
        {
            loginRepo[chatId] = auth;
        }
    }
}
