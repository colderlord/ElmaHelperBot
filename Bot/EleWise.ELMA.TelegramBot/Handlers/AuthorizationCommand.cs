using System.Collections.Generic;
using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Handlers;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;
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
        private readonly AllCommands allCommands;
        private readonly IBotWebApiService botWebApiService;
        public AuthorizationCommand(
            IAuthorizationService authorizationService, 
            IChatRepository chatRepository, 
            IBotService botService,
            IAuthRepository authRepository,
            AllCommands allCommands,
            IBotWebApiService botWebApiService) : base(botService)
        {
            this.authorizationService = authorizationService;
            this.chatRepository = chatRepository;
            this.authRepository = authRepository;
            this.allCommands = allCommands;
            this.botWebApiService = botWebApiService;
        }

        public override bool Show => false;

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
            var auth = authRepository.GetCurrentAuth(identifier);
            if (auth != null)
            {
                await BotService.Client.SendTextMessageAsync(identifier, $"Привет {m.From.FirstName} {m.From.LastName}! Вы уже авторизованы!");
                chatRepository.ResetState(identifier);
                await allCommands.HandleCommand(identifier, message);
                return;
            }
 
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
                    await((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Неуспешная авторизация! Введите корректный логин");
                }
                else
                {
                    authRepository.SetAuth(identifier, model);
                    var additional = EasterEgg(model);
                    await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, $"Авторизация прошла успешно!\r\n Хорошего дня {additional}");


                    await botWebApiService.UpdateUser(identifier.ToString(), model);

                    chatRepository.ResetState(identifier);

                    await allCommands.HandleCommand(identifier, message);
                }
            }
        }

        private string EasterEgg(Auth auth)
        {
            var id = auth.CurrentUserId;
            if (id == "109")
            {
                return "Mr. No";
            }
            if (id == "129")
            {
                return "Boss. Сыграешь на гитаре после выступления?";
            }
            if (id == "103")
            {
                return "Антон. Наша команда с тобой не знакома :(";
            }
            return string.Empty;
        }
    }
}
