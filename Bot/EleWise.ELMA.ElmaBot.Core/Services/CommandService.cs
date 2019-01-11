using EleWise.ELMA.ElmaBot.Core.Handlers;
using EleWise.ELMA.ElmaBot.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EleWise.ELMA.ElmaBot.Core.Services
{
    public sealed class CommandService : ICommandService
    {
        private readonly IBotService botService;
        private readonly IServiceProvider serviceProvider;
        private readonly IEnumerable<ICommand> commands;
        private readonly UnknownCommand unknownCommand;
        private readonly AllCommands allCommands;

        public CommandService(IBotService botService, IServiceProvider serviceProvider, UnknownCommand unknownCommand, AllCommands allCommands)
        {
            this.botService = botService;
            this.serviceProvider = serviceProvider;
            this.unknownCommand = unknownCommand;
            this.allCommands = allCommands;
            commands = serviceProvider.GetServices<ICommand>();
        }

        public ICommand GetCommandFromText(string text)
        {
            text = text.Trim();
            if (!text.StartsWith('/'))
            {
                return null;
            }
            if (text == "/")
            {
                return allCommands;
            }
            var blankIndex = text.IndexOf(' ');
            if (blankIndex < 0) blankIndex = text.Length;
            var commandName = text.Substring(1, blankIndex - 1);
            var command = commands.FirstOrDefault(a => a.CommandName.Equals(commandName, StringComparison.InvariantCultureIgnoreCase));
            return command ?? unknownCommand;
        }
    }
}
