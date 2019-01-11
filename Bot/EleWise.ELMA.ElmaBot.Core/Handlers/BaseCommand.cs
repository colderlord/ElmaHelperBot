using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Models;
using EleWise.ELMA.ElmaBot.Core.Services;

namespace EleWise.ELMA.ElmaBot.Core.Handlers
{
    public abstract class BaseCommand : ICommand
    {
        public BaseCommand(IBotService botService)
        {
            BotService = botService;
        }

        public abstract string CommandName { get; }

        public virtual string CommandDescription { get; }

        public abstract Task HandleCommand(long identifier, string text);

        public virtual Task HandleCommandState(long identifier, string chatState, string text)
        {
            return Task.Delay(0);
        }

        protected IBotService BotService { get; }
    }
}
