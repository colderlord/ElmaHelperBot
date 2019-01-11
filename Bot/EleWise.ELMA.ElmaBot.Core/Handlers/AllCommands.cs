using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Models;
using EleWise.ELMA.ElmaBot.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EleWise.ELMA.ElmaBot.Core.Handlers
{
    public class AllCommands : BaseCommand
    {
        private readonly IServiceProvider serviceProvider;
        private IEnumerable<ICommand> commands;

        public AllCommands(IBotService botService, IServiceProvider serviceProvider) : base(botService)
        {
            this.serviceProvider = serviceProvider;
        }

        public override string CommandName => "all";

        public override Task HandleCommand(long identifier, string text)
        {
            if (commands == null)
            {
                commands = serviceProvider.GetServices<ICommand>().Where(c => c.CommandName != "unknown" && c.CommandName != "all" && c.CommandName != "start");
            }
            var commandListString = $"Список команд бота: ";
            foreach(var command in commands)
            {
                commandListString += $"\r\n /{command.CommandName} {command.CommandDescription}";
            }
            return BotService.Client.SendTextMessageAsync(identifier, commandListString);
        }
    }
}
