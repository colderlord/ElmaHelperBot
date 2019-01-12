using System.Collections.Generic;
using EleWise.ELMA.ElmaBot.Core.Models;
using EleWise.ELMA.ElmaBot.Core.Services;
using EleWise.ELMA.TelegramBot.Models;

namespace EleWise.ELMA.TelegramBot.Services
{
    public class ChatRepository : IChatRepository
    {
        // Репозиторий. Ключ: идентификатор чата. Значение: пара из команды и состояния
        private Dictionary<long, IChatState> chatRepo;

        public ChatRepository()
        {
            chatRepo = new Dictionary<long, IChatState>();
        }

        public IChatState GetCurrentState(long chatId)
        {
            if (chatRepo.TryGetValue(chatId, out IChatState state))
            {
                return state;
            }
            return null;
        }

        public void SetState(long chatId, ICommand command, string state)
        {
            chatRepo[chatId] = new TelegramChatState(command, state);
        }

        public void ResetState(long chatId)
        {
            chatRepo[chatId] = null;
        }
    }
}
