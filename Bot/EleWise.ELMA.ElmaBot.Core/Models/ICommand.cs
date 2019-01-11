using System.Threading.Tasks;

namespace EleWise.ELMA.ElmaBot.Core.Models
{
    /// <summary>
    /// Команда
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Имя команды
        /// </summary>
        string CommandName { get; }

        /// <summary>
        /// Описание команды
        /// </summary>
        string CommandDescription { get; }

        /// <summary>
        /// Обработать команду
        /// </summary>
        /// <param name="identifier">Идентификатор чата</param>
        Task HandleCommand(long identifier, string text);

        /// <summary>
        /// Обработать состояние команды чата
        /// </summary>
        /// <param name="identifier">Идентификатор чата</param>
        /// <param name="chatState">Состояние чата</param>
        /// <param name="text">Текст пользователя</param>
        /// <returns></returns>
        Task HandleCommandState(long identifier, string chatState, string text);
    }
}
