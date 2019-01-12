using EleWise.ELMA.ElmaBot.Core.Models;
using EleWise.ELMA.ElmaBot.Core.Services;
using EleWise.ELMA.TelegramBot.Client;
using Microsoft.Extensions.Options;
using MihaZupan;

namespace EleWise.ELMA.TelegramBot.Services
{
    public class BotService : IBotService
    {
        private readonly BotConfiguration config;

        public BotService(IOptions<BotConfiguration> config)
        {
            this.config = config.Value;
            Client = string.IsNullOrEmpty(this.config.Socks5Host)
                ? new TelegramBotClient(this.config.BotToken)
                : new TelegramBotClient(
                    this.config.BotToken,
                    new HttpToSocks5Proxy(this.config.Socks5Host, this.config.Socks5Port, this.config.User, this.config.Password)); 
        }

        public IBotClient Client { get; }
    }
}
