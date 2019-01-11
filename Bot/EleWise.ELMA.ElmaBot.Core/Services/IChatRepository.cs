using EleWise.ELMA.ElmaBot.Core.Models;

namespace EleWise.ELMA.ElmaBot.Core.Services
{
    public interface IChatRepository
    {
        IChatState GetCurrentState(long chatId);

        void SetState(long chatId, ICommand command, string state);

        void ResetState(long chatId);
    }
}
