using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Handlers;
using EleWise.ELMA.ElmaBot.Core.Rest.Models;
using EleWise.ELMA.ElmaBot.Core.Rest.Services;
using EleWise.ELMA.ElmaBot.Core.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EleWise.ELMA.TelegramBot.Handlers
{
    public class StartProcessCommand : BaseCommand
    {
        private const string chooseProcess = "chooseProcess";
        private const string startProcess = "startProcess";
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
            IBotService botService) : base(botService)
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
            // Костыль
            var markup = new InlineKeyboardMarkup(new[]
             {
                    InlineKeyboardButton.WithCallbackData("Заявка на отгул", otgul)
            });
            await ((TelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Запустить процесс", replyMarkup: markup);
            chatRepository.SetState(identifier, this, chooseProcess);
        }

        public override async Task HandleCommandState(long identifier, string chatState, object message, string data)
        {
            var m = message as Message;
            switch (chatState)
            {
                case chooseProcess:
                {
                    // Должны были выбрать процесс - это токен
                    var body = new StartProcessBody
                    {
                        ProcessToken = "d3a935f7-9980-4877-a55a-8222b6073566",
                        ProcessName = "FromBot",
                        Context = new ExpandoObject()
                    };
                    startProcessRepository.SetStartProcessBody(identifier, body);
                    ((ICollection<KeyValuePair<string, object>>)body.Context).Add(new KeyValuePair<string, object>("Bot", true));

                    await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Введите дату начала:");
                    chatRepository.SetState(identifier, this, startDate);

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
                            await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Введите дату окончания ДД.ММ.ГГГГ ЧЧ:ММ");
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
                            await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Введите причину отгула в формате ДД.ММ.ГГГГ ЧЧ:ММ");
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

                        // Надо переместить
                        var markup = new InlineKeyboardMarkup(new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Да", "+"),
                            InlineKeyboardButton.WithCallbackData("Нет", "-")
                        });
                        await ((TelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Запустить процесс", replyMarkup: markup);
                        chatRepository.SetState(identifier, this, startProcess);
                    }
                    break;
                }
                case startProcess:
                {
                    if (data != "+")
                    {
                        await ((Models.ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Отменяю запуск");
                        chatRepository.ResetState(identifier);
                        break;
                    }

                    var current = startProcessRepository.GetCurrentStartProcessBody(identifier);
                    if (current == null)
                    {
                        break;
                    }

                    await ((ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Пытаюсь запустить процесс, ожидайте");

                    var auth = authRepository.GetCurrentAuth(identifier);
                    // Костыль
                    var result = await startProcessService.StartProcessAsync(current, auth);
                    if (result != null)
                    {
                        await ((Models.ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Процесс успешно запущен");
                        chatRepository.ResetState(identifier);
                    }
                    else
                    {
                        await ((Models.ITelegramBotClient)BotService.Client).SendTextMessageAsync(identifier, "Процесс не запущен. Попробовать снова?");
                    }

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
