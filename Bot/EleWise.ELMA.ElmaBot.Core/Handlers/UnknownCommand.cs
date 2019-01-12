using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Services;

namespace EleWise.ELMA.ElmaBot.Core.Handlers
{
    public class UnknownCommand : BaseCommand
    {
        public UnknownCommand(IBotService botService) : base(botService)
        {
        }

        public override string CommandName => "unknown";

        public override Task HandleCommand(long identifier, object message)
        {
            return BotService.Client.SendTextMessageAsync(identifier, $"Указанной команды не существует! Для просмотра всех команд введите '/'");
        }
    }
}
