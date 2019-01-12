using System.Reflection;
using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Attributes;
using EleWise.ELMA.ElmaBot.Core.Services;
using EleWise.ELMA.TelegramBot.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EleWise.ELMA.TelegramBot.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService botService;
        private readonly ICommandService commandService;
        private readonly IChatRepository chatRepository;
        private readonly IAuthRepository authRepository;
        private readonly AuthorizationCommand authorizationCommand;

        public UpdateService(IBotService botService, ICommandService commandService, IChatRepository chatRepository, IAuthRepository authRepository, AuthorizationCommand authorizationCommand)
        {
            this.botService = botService;
            this.commandService = commandService;
            this.chatRepository = chatRepository;
            this.authRepository = authRepository;
            this.authorizationCommand = authorizationCommand;
        }

        public async Task EchoAsync(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                {
                    var message = update.Message;
                    var chatId = message.Chat.Id;
                    var currentState = chatRepository.GetCurrentState(chatId);
                    var text = message.Text;
                    if (currentState != null)
                    {
                        var stateCommand = currentState.Command;

                        var authorized = authRepository.Authorized(chatId);
                        var needAuth = stateCommand.GetType().GetCustomAttribute<AuthorizationAttribute>() != null;
                        if (needAuth && !authorized)
                        {
                            await authorizationCommand.HandleCommand(chatId, message);
                            break;
                        }

                        await currentState.Command.HandleCommandState(chatId, currentState.State, message, "");
                        break;
                    }
                    var command = commandService.GetCommandFromText(text);
                    if (command != null)
                    {
                        var authorized = authRepository.Authorized(chatId);
                        var needAuth = command.GetType().GetCustomAttribute<AuthorizationAttribute>() != null;
                        if (needAuth && !authorized)
                        {
                            await authorizationCommand.HandleCommand(chatId, message);
                            break;
                        }

                        await command.HandleCommand(chatId, message);
                    }
                    else
                    {
                        // Возвращаем текст
                        await botService.Client.SendTextMessageAsync(chatId, $"Вы сказали: {text}");
                    }
                    break;
                }
                case UpdateType.CallbackQuery:
                {
                    // Обработка callback
                    var message = update.CallbackQuery.Message;
                    var chatId = message.Chat.Id;
                    
                    var currentState = chatRepository.GetCurrentState(chatId);
                    
                    if (currentState != null)
                    {
                        var command = currentState.Command;

                        var authorized = authRepository.Authorized(chatId);
                        var needAuth = command.GetType().GetCustomAttribute<AuthorizationAttribute>() != null;
                        if (needAuth && !authorized)
                        {
                            await authorizationCommand.HandleCommand(chatId, message);
                            break;
                        }

                        await command.HandleCommandState(chatId, currentState.State, message, update.CallbackQuery.Data);
                    }
                    break;
                }
            }
        }
    }
}
