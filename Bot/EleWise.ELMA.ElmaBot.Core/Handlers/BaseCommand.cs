using System.Collections.Generic;
using System.Linq;
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

        public virtual bool Show => true;

        public virtual IEnumerable<string> AlternativeCommand => Enumerable.Empty<string>();

        public virtual string CommandDescription { get; }

        public abstract Task HandleCommand(long identifier, object message);

        public virtual Task HandleCommandState(long identifier, string chatState, object message, string data)
        {
            return Task.Delay(0);
        }

        protected IBotService BotService { get; }
    }
}
