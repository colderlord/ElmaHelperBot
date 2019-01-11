using System.Threading.Tasks;

namespace EleWise.ELMA.ElmaBot.Core.Models
{
    /// <summary>
    /// Интерфейс клиента бота
    /// </summary>
    public interface IBotClient
    {
        Task SendTextMessageAsync(long identifier, string text);

        Task SendTextMessageAsync(int chatId, string text);
    }
}
