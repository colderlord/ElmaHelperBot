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

        public override Task HandleCommand(long identifier, string text)
        {
            return BotService.Client.SendTextMessageAsync(identifier, $"Указанной команды {text} не существует! Для просмотра всех команд введите '/'");
        }
    }
}
