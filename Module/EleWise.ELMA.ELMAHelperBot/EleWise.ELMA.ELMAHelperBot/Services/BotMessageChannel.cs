using System;
using EleWise.ELMA.ComponentModel;
using EleWise.ELMA.Messaging;
using EleWise.ELMA.Security.Models;
using MihaZupan;
using Telegram.Bot;

namespace EleWise.ELMA.ELMAHelperBot.Services
{
    [Component]
    public class BotMessageChannel : IMessageChannel
    {
        private readonly Guid uid = new Guid("{B2D745F9-9624-40c4-9C07-ABF44281F066}");
        private readonly TelegramBotClient client;
        public BotMessageChannel()
        {
            // Сделать точку расширения для хранения настроек
            client = new TelegramBotClient(
                    "740200329:AAFJY2AD9oekX7SHfJUiN4C3SaZSx8o05cw",
                    new HttpToSocks5Proxy("d2a5e5.reconnect.rocks", 1080, "3559738", "11a1fcc2"));
        }

        
        public Guid Uid
        {
            get { return uid; }
        }

        public string Name
        {
            get { return "TelegramBotChannel"; }
        }

        public string DisplayName
        {
            get { return "Канал отправки сообщений в телеграм"; }
        }

        public bool Default
        {
            get { return true; }
        }

        public void Send(IMessage message)
        {
            Contract.ArgumentNotNull(message, nameof(message));

            //Проверка получателя
            var recipient = message.Recipient as IUser;
            if (recipient == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(recipient.JabberID))
            {
                return;
            }
            var text = SR.T($"Сообщение из ELMA:\r\n Тема: {message.Subject}\r\n Текст сообщения: {message.FullMessageText}");
            client.SendTextMessageAsync(long.Parse(recipient.JabberID), text);
        }
    }
}
