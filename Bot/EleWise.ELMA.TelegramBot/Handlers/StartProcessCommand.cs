using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Attributes;
using EleWise.ELMA.ElmaBot.Core.Handlers;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;
using EleWise.ELMA.ElmaBot.Core.Rest.Services;
using EleWise.ELMA.ElmaBot.Core.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EleWise.ELMA.TelegramBot.Handlers
{
    [Authorization]
    public class StartProcessCommand : BaseCommand
    {
        private const string chooseProcess = "chooseProcess";
        private const string startProcess = "startProcess";
        private const string startProcessQuestion = "startProcessQuestion";
        private const string otgul = "otgul";
        private const string startDate = "startDate";
        private const string endDate = "endDate";
        private const string reason = "reason";

        private readonly IChatRepository chatRepository;
        private readonly IStartProcessService startProcessService;
        private readonly IAuthRepository authRepository;
        private readonly IStartProcessRepository startProcessRepository;
        public StartProcessCommand(
            IChatRepository chatRepository,
            IStartProcessService startProcessService,
            IAuthRepository authRepository,
            IStartProcessRepository startProcessRepository,
            IBotService botService,
            IBotWebApiService botWebApiService) : base(botService)
        {
            this.chatRepository = chatRepository;
            this.startProcessService = startProcessService;
            this.authRepository = authRepository;
            this.startProcessRepository = startProcessRepository;
        }

        public override string CommandName => "startprocess";

        public override string CommandDescription => "Запустить процесс";

        public override async Task HandleCommand(long identifier, object message)
        {
            await ((TelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Ищу процессы, возможные для запуска");
            var auth = authRepository.GetCurrentAuth(identifier);

            var startableProcesses = await startProcessService.StartableProcesses(auth);

            var markup = new InlineKeyboardMarkup(
                startableProcesses.Processes.Select(p => InlineKeyboardButton.WithCallbackData(p.Name, p.Id.ToString()))
            );
            await ((TelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Список доступных процессов:", replyMarkup: markup);
            chatRepository.SetState(identifier, this, chooseProcess);
        }

        public override async Task HandleCommandState(long identifier, string chatState, object message, string data)
        {
            var m = message as Message;
            switch (chatState)
            {
                case chooseProcess:
                {
                    var processId = long.Parse(data);

                    // Должны были выбрать процесс - это токен
                    var body = new StartProcessBody
                    {
                        ProcessHeaderId = long.Parse(data),
                        ProcessName = "FromBot",
                        Context = new ExpandoObject()
                    };
                    startProcessRepository.SetStartProcessBody(identifier, body);

                    // Хардкод на 1 процесс
                    if (processId == 102)
                    {
                        ((ICollection<KeyValuePair<string, object>>)body.Context).Add(new KeyValuePair<string, object>("Bot", true));

                        await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Введите дату начала в формате ДД.ММ.ГГГГ ЧЧ:ММ");
                        chatRepository.SetState(identifier, this, startDate);
                    }
                    else
                    {
                        await AllDone(identifier);
                    }

                    break;
                }
                case startDate:
                {
                    var current = startProcessRepository.GetCurrentStartProcessBody(identifier);
                    if (current != null)
                    {
                        if (DateTime.TryParse(m.Text, out DateTime date))
                        {
                            ((ICollection<KeyValuePair<string, object>>)current.Context).Add(new KeyValuePair<string, object>("DateStart", date));
                            await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Введите дату окончания в формате ДД.ММ.ГГГГ ЧЧ:ММ");
                            chatRepository.SetState(identifier, this, endDate);
                        }
                        else
                        {
                            await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Дата не распознана, введите корректно");
                        }
                    }
                    break;
                }
                case endDate:
                {
                    var current = startProcessRepository.GetCurrentStartProcessBody(identifier);
                    if (current != null)
                    {
                        if (DateTime.TryParse(m.Text, out DateTime date))
                        {
                            ((ICollection<KeyValuePair<string, object>>)current.Context).Add(new KeyValuePair<string, object>("DateEnd", date));
                            await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Введите причину отгула");
                            chatRepository.SetState(identifier, this, reason);
                        }
                        else
                        {
                            await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Дата не распознана, введите корректно");
                        }
                    }
                    break;
                }
                case reason:
                {
                    var current = startProcessRepository.GetCurrentStartProcessBody(identifier);
                    if (current != null)
                    {
                        ((ICollection<KeyValuePair<string, object>>)current.Context).Add(new KeyValuePair<string, object>("Reason", m.Text));

                        chatRepository.SetState(identifier, this, startProcessQuestion);
                    }
                    break;
                }
                case startProcessQuestion:
                {
                    await AllDone(identifier);
                    break;
                }
                case startProcess:
                {
                    await StartProcess(identifier, data);
                    break;
                }
                default:
                {
                    chatRepository.ResetState(identifier);
                    break;
                }
            }
        }

        private async Task AllDone(long identifier)
        {
            var markup = new InlineKeyboardMarkup(new[]
                    {
                            InlineKeyboardButton.WithCallbackData("Да", "+"),
                            InlineKeyboardButton.WithCallbackData("Нет", "-")
                        });
            await((TelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Всё готово для запуска. Запустить процесс?", replyMarkup: markup);
            chatRepository.SetState(identifier, this, startProcess);
        }

        private async Task StartProcess(long identifier, string data)
        {
            if (data != "+")
            {
                await((Models.ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Отменяю запуск");
                chatRepository.ResetState(identifier);
                return;
            }

            var current = startProcessRepository.GetCurrentStartProcessBody(identifier);
            if (current == null)
            {
                return;
            }

            await((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Пытаюсь запустить процесс, ожидайте");

            var auth = authRepository.GetCurrentAuth(identifier);
            // Костыль
            var result = await startProcessService.StartProcessAsync(current, auth);
            if (result != null)
            {
                await((Models.ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Процесс успешно запущен");
                chatRepository.ResetState(identifier);
            }
            else
            {
                await((Models.ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Процесс не запущен. Попробовать снова?");
            }
        }
    }
}
