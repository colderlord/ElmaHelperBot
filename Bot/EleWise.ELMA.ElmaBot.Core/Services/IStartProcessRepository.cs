using EleWise.ELMA.ElmaBot.Core.Rest.Models;

namespace EleWise.ELMA.ElmaBot.Core.Services
{
    public interface IStartProcessRepository
    {
        StartProcessBody GetCurrentStartProcessBody(long chatId);

        void SetStartProcessBody(long chatId, StartProcessBody startProcessBody);
    }
}
