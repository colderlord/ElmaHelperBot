using System;
using System.Collections.Generic;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;
using EleWise.ELMA.ElmaBot.Core.Services;

namespace EleWise.ELMA.TelegramBot.Services
{
    public class ProcessContextRepository : IProcessContextRepository
    {
        private Dictionary<long, Tuple<List<ContextProcess>, ContextProcess>> repo;

        public ProcessContextRepository()
        {
            repo = new Dictionary<long, Tuple<List<ContextProcess>, ContextProcess>>();
        }

        public List<ContextProcess> GetCurrentProcessContext(long chatId)
        {
            if (repo.TryGetValue(chatId, out Tuple<List<ContextProcess>, ContextProcess> body))
            {
                return body.Item1;
            }
            return null;
        }

        public ContextProcess GetCurrentProcessEditing(long chatId)
        {
            if (repo.TryGetValue(chatId, out Tuple<List<ContextProcess>, ContextProcess> body))
            {
                return body.Item2;
            }
            return null;
        }

        public void SetCurrentProcessContext(long chatId, List<ContextProcess> newContext, ContextProcess editing)
        {
            repo[chatId] = new Tuple<List<ContextProcess>, ContextProcess>(newContext, editing);
        }
    }
}
