using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Handlers;
using EleWise.ELMA.ElmaBot.Core.Services;
using EleWise.ELMA.TelegramBot.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace EleWise.ELMA.TelegramBot.Handlers
{
    public class TestCommand : BaseCommand
    {
        private readonly IChatRepository chatRepository;
        private const string NextStep = "Next";
        private const string NextStep2 = "Next2";

        public TestCommand(IBotService botService, IChatRepository chatRepository) : base(botService)
        {
            this.chatRepository = chatRepository;
        }

        public override string CommandName => "test";

        public override string CommandDescription => "Тестовая команда";

        public override async Task HandleCommand(long identifier, object message)
        {
            var markup = new InlineKeyboardMarkup(new []
            {
                    InlineKeyboardButton.WithCallbackData("Privet", "privet"),
                    InlineKeyboardButton.WithCallbackData("Hello", "hello"),
                    InlineKeyboardButton.WithCallbackData("Zdarova", "zdarova")
            });
            await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Hello", replyMarkup: markup);
            chatRepository.SetState(identifier, this, NextStep);
        }

        public override async Task HandleCommandState(long identifier, string chatState, object message, string data)
        {
            switch(chatState)
            {
                case NextStep:
                {
                    await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "NextStep");
                    chatRepository.SetState(identifier, this, NextStep2);
                    break;
                }
                case NextStep2:
                {
                    await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "NextStep2");
                    chatRepository.ResetState(identifier);
                    break;
                }
            }
        }
    }
}
