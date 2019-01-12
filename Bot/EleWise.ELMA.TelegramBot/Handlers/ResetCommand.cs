using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Handlers;
using EleWise.ELMA.ElmaBot.Core.Services;
using EleWise.ELMA.TelegramBot.Models;

namespace EleWise.ELMA.TelegramBot.Handlers
{
    public class ResetCommand : BaseCommand
    {
        private readonly IChatRepository chatRepository;
        public ResetCommand(IBotService botService, IChatRepository chatRepository) : base(botService)
        {
            this.chatRepository = chatRepository;
        }

        public override string CommandName => "reset";

        public override Task HandleCommand(long identifier, object message)
        {
            chatRepository.ResetState(identifier);

            return ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Состояние сброшено");
        }
    }
}
