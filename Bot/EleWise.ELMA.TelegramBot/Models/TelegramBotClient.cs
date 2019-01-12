using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EleWise.ELMA.TelegramBot.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EleWise.ELMA.TelegramBot.Client
{
    public sealed class TelegramBotClient : Telegram.Bot.TelegramBotClient, ITelegramBotClient
    {
        public TelegramBotClient(string token, HttpClient httpClient = null) : base(token, httpClient)
        {
        }

        public TelegramBotClient(string token, IWebProxy webProxy) : base(token, webProxy)
        {
        }

        public Task SendTextMessageAsync(long identifier, string text)
        {
            return SendTextMessageAsync(identifier, text, null);
        }

        public Task SendTextMessageAsync(int chatId, string text)
        {
            return SendTextMessageAsync(chatId, text, null);
        }

        public Task SendTextMessageAsync(long identifier, string text, IReplyMarkup replyMarkup = null)
        {
            ChatId chat = identifier;
            return SendTextMessageAsync(chat, text, replyMarkup: replyMarkup);
        }

        public Task SendTextMessageAsync(int chatId, string text, IReplyMarkup replyMarkup = null)
        {
            ChatId chat = chatId;
            return SendTextMessageAsync(chat, text, replyMarkup: replyMarkup);
        }
    }
}
