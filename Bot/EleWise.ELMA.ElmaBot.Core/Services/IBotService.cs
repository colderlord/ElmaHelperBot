using EleWise.ELMA.ElmaBot.Core.Models;

namespace EleWise.ELMA.ElmaBot.Core.Services
{
    /// <summary>
    /// Сервис для работы с ботом
    /// </summary>
    public interface IBotService
    {
        /// <summary>
        /// Клиент бота
        /// </summary>
        IBotClient Client { get; }
    }
}
