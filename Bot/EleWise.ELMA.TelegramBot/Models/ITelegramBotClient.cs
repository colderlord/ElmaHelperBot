using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace EleWise.ELMA.TelegramBot.Models
{
    public interface ITelegramBotClient : IBotClient
    {
        Task SendTextMessageAsync(long identifier, string text, IReplyMarkup replyMarkup = null);

        Task SendTextMessageAsync(int chatId, string text, IReplyMarkup replyMarkup = null);
    }
}
