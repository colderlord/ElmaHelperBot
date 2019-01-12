using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Handlers;
using EleWise.ELMA.ElmaBot.Core.Rest.Services;
using EleWise.ELMA.ElmaBot.Core.Services;
using EleWise.ELMA.TelegramBot.Models;
using Telegram.Bot.Types;

namespace EleWise.ELMA.TelegramBot.Handlers
{
    public class AuthorizationCommand : BaseCommand
    {
        private const string checkLogin = "checkLogin";

        private readonly IAuthorizationService authorizationService;
        private readonly IChatRepository chatRepository;
        private readonly IAuthRepository authRepository;
        public AuthorizationCommand(
            IAuthorizationService authorizationService, 
            IChatRepository chatRepository, 
            IBotService botService,
            IAuthRepository authRepository) : base(botService)
        {
            this.authorizationService = authorizationService;
            this.chatRepository = chatRepository;
            this.authRepository = authRepository;
        }

        public override string CommandName => "auth";

        public override string CommandDescription => "Команда авторизации";

        public override IEnumerable<string> AlternativeCommand => new[]
        {
            "привет",
            "привет.",
            "привет!",
            "ghbdtn"
        };

        public override async Task HandleCommand(long identifier, object message)
        {
            var m = message as Message;
            await BotService.Client.SendTextMessageAsync(identifier, $"Привет, {m.From.FirstName} {m.From.LastName}!");
            await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Введите ваш логин:");
            chatRepository.SetState(identifier, this, checkLogin);
        }

        public override async Task HandleCommandState(long identifier, string chatState, object message, string data)
        {
            var m = message as Message;
            if (chatState == checkLogin)
            {
                var model = await authorizationService.LoginWithUserName(m.Text, null);
                if (model == null)
                {
                    await((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Неуспешная авторизация! Попробуем снова?");
                }
                else
                {
                    authRepository.SetAuth(identifier, model);
                    await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Авторизация прошла успешно!");
                    chatRepository.ResetState(identifier);
                }
            }
        }
    }
}
