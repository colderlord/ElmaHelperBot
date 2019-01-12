using System.Collections.Generic;
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
        /// Альтернативные команды
        /// </summary>
        IEnumerable<string> AlternativeCommand { get; }

        /// <summary>
        /// Описание команды
        /// </summary>
        string CommandDescription { get; }

        /// <summary>
        /// Отображать команду
        /// </summary>
        bool Show { get; }

        /// <summary>
        /// Обработать команду
        /// </summary>
        /// <param name="identifier">Идентификатор чата</param>
        /// <param name="message">Сообщение</param>
        Task HandleCommand(long identifier, object message);

        /// <summary>
        /// Обработать состояние команды чата
        /// </summary>
        /// <param name="identifier">Идентификатор чата</param>
        /// <param name="chatState">Состояние чата</param>
        /// <param name="message">Сообщение</param>
        /// <param name="data">Данные</param>
        Task HandleCommandState(long identifier, string chatState, object message, string data);
    }
}
