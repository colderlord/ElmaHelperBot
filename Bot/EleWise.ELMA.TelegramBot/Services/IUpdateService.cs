using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace EleWise.ELMA.TelegramBot.Services
{
    /// <summary>
    /// Сервис обновления информации бота
    /// </summary>
    public interface IUpdateService
    {
        /// <summary>
        /// Асинхронное выполнение обновления
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        Task EchoAsync(Update update);
    }
}
