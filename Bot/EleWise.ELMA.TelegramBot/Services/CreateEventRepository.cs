using System.Collections.Generic;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;
using EleWise.ELMA.ElmaBot.Core.Services;

namespace EleWise.ELMA.TelegramBot.Services
{
    public class CreateEventRepository : ICreateEventRepository
    {
        private Dictionary<long, EventCreatorModel> repo;
        public CreateEventRepository()
        {
            repo = new Dictionary<long, EventCreatorModel>();
        }

        public EventCreatorModel GetCurrentEventCreatorModel(long chatId)
        {
            if (repo.TryGetValue(chatId, out EventCreatorModel body))
            {
                return body;
            }
            return null;
        }

        public void SetEventCreatorModel(long chatId, EventCreatorModel eventCreatorBody)
        {
            repo[chatId] = eventCreatorBody;
        }
    }
}
