using System.Threading.Tasks;
using EleWise.ELMA.ElmaBot.Core.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EleWise.ELMA.TelegramBot.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService botService;
        private readonly ICommandService commandService;
        private readonly IChatRepository chatRepository;

        public UpdateService(IBotService botService, ICommandService commandService, IChatRepository chatRepository)
        {
            this.botService = botService;
            this.commandService = commandService;
            this.chatRepository = chatRepository;
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
                        await currentState.HandleCurrentState(chatId, message, "");
                        break;
                    }
                    var command = commandService.GetCommandFromText(text);
                    if (command != null)
                    {
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
                        await currentState.HandleCurrentState(chatId, message, update.CallbackQuery.Data);
                    }
                    break;
                }
            }

            //if (update.Type != UpdateType.Message)
            //{
            //    return;
            //}

            //var message = update.Message;

            //ICommand command = commandService.GetCommandFromMessageText(message.Text);
            //if (command != null)
            //{
            //    await command.HandleCommand(message.Chat.Id, message.Text);
            //}
            //else
            //{
            //    // Возвращаем текст
            //    await botService.Client.SendTextMessageAsync(message.Chat.Id, $"Вы сказали: {message.Text}");
            //}
            //else if (message.Type == MessageType.Photo)
            //    //{
            //    //    // Download Photo
            //    //    var fileId = message.Photo.LastOrDefault()?.FileId;
            //    //    var file = await _botService.Client.GetFileAsync(fileId);

            //    //    var filename = file.FileId + "." + file.FilePath.Split('.').Last();

            //    //    using (var saveImageStream = System.IO.File.Open(filename, FileMode.Create))
            //    //    {
            //    //        await _botService.Client.DownloadFileAsync(file.FilePath, saveImageStream);
            //    //    }

            //    //    await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Thx for the Pics");
            //    //}
        }
    }
}
