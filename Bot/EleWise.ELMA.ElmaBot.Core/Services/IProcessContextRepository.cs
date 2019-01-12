using System.Collections.Generic;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;

namespace EleWise.ELMA.ElmaBot.Core.Services
{
    public interface IProcessContextRepository
    {
        List<ContextProcess> GetCurrentProcessContext(long chatId);

        ContextProcess GetCurrentProcessEditing(long chatId);

        void SetCurrentProcessContext(long chatId, List<ContextProcess> newContext, ContextProcess editing);
    }
}
