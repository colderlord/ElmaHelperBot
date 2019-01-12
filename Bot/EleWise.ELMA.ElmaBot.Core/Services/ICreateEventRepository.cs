using EleWise.ELMA.ElmaBot.Core.Rest.Models;

namespace EleWise.ELMA.ElmaBot.Core.Services
{
    public interface ICreateEventRepository
    {
        EventCreatorModel GetCurrentEventCreatorModel(long chatId);

        void SetEventCreatorModel(long chatId, EventCreatorModel eventCreatorBody);
    }
}
