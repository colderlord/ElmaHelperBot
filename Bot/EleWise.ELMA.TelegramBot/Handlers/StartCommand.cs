using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Handlers;
using EleWise.ELMA.ElmaBot.Core.Services;
using Telegram.Bot.Types;

namespace EleWise.ELMA.TelegramBot.Handlers
{
    public class StartCommand : BaseCommand
    {
        private readonly AuthorizationCommand authorizationCommand;

        public StartCommand(AuthorizationCommand authorizationCommand, IBotService botService) : base(botService)
        {
            this.authorizationCommand = authorizationCommand;
        }

        public override string CommandName => "start";

        public override async Task HandleCommand(long identifier, object message)
        {
            var m = message as Message;
            await authorizationCommand.HandleCommand(identifier, "");
        }
    }
}
