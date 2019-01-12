using System.Collections.Generic;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;
using EleWise.ELMA.ElmaBot.Core.Services;

namespace EleWise.ELMA.TelegramBot.Services
{
    public class StartProcessRepository : IStartProcessRepository
    {
        private Dictionary<long, StartProcessBody> repo;

        public StartProcessRepository()
        {
            repo = new Dictionary<long, StartProcessBody>();
        }

        public StartProcessBody GetCurrentStartProcessBody(long chatId)
        {
            if (repo.TryGetValue(chatId, out StartProcessBody body))
            {
                return body;
            }
            return null;
        }

        public void SetStartProcessBody(long chatId, StartProcessBody startProcessBody)
        {
            repo[chatId] = startProcessBody;
        }
    }
}
