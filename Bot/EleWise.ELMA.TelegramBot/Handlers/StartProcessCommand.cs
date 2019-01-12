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
        private const string writeContext = "writeContext";
        private const string startProcessQuestion = "startProcessQuestion";
        private const string otgul = "otgul";
        private const string startDate = "startDate";
        private const string endDate = "endDate";
        private const string reason = "reason";

        private Dictionary<long, Dictionary<ContextProcess, object>> dict;

        private readonly IChatRepository chatRepository;
        private readonly IStartProcessService startProcessService;
        private readonly IAuthRepository authRepository;
        private readonly IStartProcessRepository startProcessRepository;
        private readonly IBotWebApiService botWebApiService;
        private readonly IProcessContextRepository processContextRepository;
        public StartProcessCommand(
            IChatRepository chatRepository,
            IStartProcessService startProcessService,
            IAuthRepository authRepository,
            IStartProcessRepository startProcessRepository,
            IBotService botService,
            IBotWebApiService botWebApiService,
            IProcessContextRepository processContextRepository) : base(botService)
        {
            this.chatRepository = chatRepository;
            this.startProcessService = startProcessService;
            this.authRepository = authRepository;
            this.startProcessRepository = startProcessRepository;
            this.botWebApiService = botWebApiService;
            this.processContextRepository = processContextRepository;
            dict = new Dictionary<long, Dictionary<ContextProcess, object>>();
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
            var auth = authRepository.GetCurrentAuth(identifier);
            switch (chatState)
            {
                case chooseProcess:
                {
                    var processId = long.Parse(data);

                    var context = await botWebApiService.GetProcessContext(processId);
                    processContextRepository.SetCurrentProcessContext(identifier, context, null);

                    // Должны были выбрать процесс - это токен
                    var body = new StartProcessBody
                    {
                        ProcessHeaderId = long.Parse(data),
                        ProcessName = "FromBot",
                        Context = new ExpandoObject()
                    };
                    startProcessRepository.SetStartProcessBody(identifier, body);

                    if (context.Count == 0)
                    {
                        await AllDone(identifier);
                        break;
                    }
                    else
                    {
                        if (processId == 102)
                        {
                            ((ICollection<KeyValuePair<string, object>>)body.Context).Add(new KeyValuePair<string, object>("Bot", true));
                        }

                        await WriteContext(identifier, m, null);
                        chatRepository.SetState(identifier, this, writeContext);

                        break;
                    }

                    // Хардкод на 1 процесс
                    //if (processId == 102)
                    //{
                    //    ((ICollection<KeyValuePair<string, object>>)body.Context).Add(new KeyValuePair<string, object>("Bot", true));

                    //    await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Введите дату начала в формате ДД.ММ.ГГГГ ЧЧ:ММ");
                    //    chatRepository.SetState(identifier, this, startDate);
                    //}
                    //else
                    //{
                    //    await AllDone(identifier);
                    //}

                    //break;
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
                        await AllDone(identifier);
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

                case writeContext:
                {
                    await WriteContext(identifier, m, m.Text);
                    break;
                }
                default:
                {
                    chatRepository.ResetState(identifier);
                    break;
                }
            }
        }

        private async Task WriteContext(long identifier, Message message, string data)
        {
            var editing = processContextRepository.GetCurrentProcessEditing(identifier);

            List<ContextProcess> context = processContextRepository.GetCurrentProcessContext(identifier);
            var current = startProcessRepository.GetCurrentStartProcessBody(identifier);
            if (current != null)
            {
                if (editing == null)
                {
                    var item = context.FirstOrDefault();
                    await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, $"Заполните переменную \"{item.DisplayName}\" типа \"{item.Type}\"");
                    processContextRepository.SetCurrentProcessContext(identifier, context, item);
                    return;
                }
                else
                {
                    if (editing.Type == "Дата / время")
                    {
                        if (!DateTime.TryParse(data, out DateTime date))
                        {
                            await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Дата не распознана, введите корректно");
                            return;
                        }
                    }

                    ((ICollection<KeyValuePair<string, object>>)current.Context).Add(new KeyValuePair<string, object>(editing.Name, data));
                    context.RemoveAt(0);
                    if (context.Count == 0)
                    {
                        await AllDone(identifier);
                    }
                    else
                    {
                        var item = context.FirstOrDefault();
                        await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, $"Заполните переменную \"{item.DisplayName}\" типа \"{item.Type}\"");
                        processContextRepository.SetCurrentProcessContext(identifier, context, item);
                    }
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
