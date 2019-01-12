using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Handlers;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;
using EleWise.ELMA.ElmaBot.Core.Rest.Services;
using EleWise.ELMA.ElmaBot.Core.Services;
using EleWise.ELMA.TelegramBot.Client;
using EleWise.ELMA.TelegramBot.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EleWise.ELMA.TelegramBot.Handlers
{
    public class EventCommand : BaseCommand
    {
        private const string writeParams = "writeParams";
        private const string writeDate = "writeDate";
        private const string timeFrom = "timeFrom";
        private const string timeTo = "timeTo";
        private const string subject = "subject";
        private const string end = "end";

        private readonly IChatRepository chatRepository;
        private readonly ICreateEventRepository createEventRepository;
        private readonly IAuthRepository authRepository;
        private readonly IBotWebApiService botWebApiService;
        public EventCommand(
            IChatRepository chatRepository,
            ICreateEventRepository createEventRepository,
            IAuthRepository authRepository,
            IBotWebApiService botWebApiService,
            IBotService botService) : base(botService)
        {
            this.authRepository = authRepository;
            this.chatRepository = chatRepository;
            this.createEventRepository = createEventRepository;
            this.botWebApiService = botWebApiService;
        }

        public override string CommandName => "createevent";

        public override string CommandDescription => "Создать событие";

        public override async Task HandleCommand(long identifier, object message)
        {
            var markup = new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData("Да", "+"),
                InlineKeyboardButton.WithCallbackData("Нет", "-")
            });
            await ((TelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Создаем событие?", replyMarkup: markup);
            chatRepository.SetState(identifier, this, writeParams);
        }

        public override async Task HandleCommandState(long identifier, string chatState, object message, string data)
        {
            var m = message as Message;
            var auth = authRepository.GetCurrentAuth(identifier);
            switch (chatState)
            {
                case writeParams:
                {
                    if (data == "+")
                    {
                        var body = new EventCreatorModel();
                        createEventRepository.SetEventCreatorModel(identifier, body);
                        chatRepository.SetState(identifier, this, writeDate);
                        await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Введите дату в формате ДД.ММ");
                    }
                    else
                    {
                        await((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Не больно то и хотелось");
                        chatRepository.ResetState(identifier);
                    }
                    break;
                }
                case writeDate:
                {
                    var current = createEventRepository.GetCurrentEventCreatorModel(identifier);
                    if (current != null)
                    {
                        current.Date = m.Text;
                        //createEventRepository.SetEventCreatorModel(identifier, current);
                        await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Введите время начала в формате ЧЧ:ММ");
                        chatRepository.SetState(identifier, this, timeFrom);
                    }
                    else
                    {
                        chatRepository.ResetState(identifier);
                    }
                    break;
                }
                case timeFrom:
                {
                    var current = createEventRepository.GetCurrentEventCreatorModel(identifier);
                    if (current != null)
                    {
                        current.TimeFrom = m.Text;
                        //createEventRepository.SetEventCreatorModel(identifier, current);
                        await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Введите время окончания в формате ЧЧ:ММ");
                        chatRepository.SetState(identifier, this, timeTo);
                    }
                    else
                    {
                        chatRepository.ResetState(identifier);
                    }
                    break;
                }
                case timeTo:
                {
                    var current = createEventRepository.GetCurrentEventCreatorModel(identifier);
                    if (current != null)
                    {
                        current.TimeTo = m.Text;
                        //createEventRepository.SetEventCreatorModel(identifier, current);
                        await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Введите тему события");
                        chatRepository.SetState(identifier, this, subject);
                    }
                    else
                    {
                        chatRepository.ResetState(identifier);
                    }
                    break;
                }
                case subject:
                {
                    var current = createEventRepository.GetCurrentEventCreatorModel(identifier);
                    if (current != null)
                    {
                        current.Subject = m.Text;

                        var result = await botWebApiService.EventCreate(current);
                        if (result)
                        {
                            await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Событие успешно создано!");
                        }
                        else
                        {
                            await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Событие не создано :(");
                        }
                    }
                    chatRepository.ResetState(identifier);
                    break;
                }
                default:
                {
                    chatRepository.ResetState(identifier);
                    break;
                }
            }
        }
    }
}
