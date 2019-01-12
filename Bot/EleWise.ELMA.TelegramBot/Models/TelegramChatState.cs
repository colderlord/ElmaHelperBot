using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Models;

namespace EleWise.ELMA.TelegramBot.Models
{
    public class TelegramChatState : IChatState
    {
        public TelegramChatState(ICommand command) : this(command, null)
        {
        }

        public TelegramChatState(ICommand command, string state)
        {
            Command = command;
            State = state;
        }

        public ICommand Command { get; }
        public string State { get; }

        public Task HandleCurrentState(long identifier, object message, string data)
        {
            return Command.HandleCommandState(identifier, State, message, data);
        }
    }
}
