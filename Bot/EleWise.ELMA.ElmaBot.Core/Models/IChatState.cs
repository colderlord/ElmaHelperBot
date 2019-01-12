using System.Threading.Tasks;

namespace EleWise.ELMA.ElmaBot.Core.Models
{
    /// <summary>
    /// Состояние чата
    /// </summary>
    public interface IChatState
    {
        /// <summary>
        /// Текущая команда
        /// </summary>
        ICommand Command { get; }

        /// <summary>
        /// Текущее состояние
        /// </summary>
        string State { get; }

        /// <summary>
        /// Обработать текущее состояние
        /// </summary>
        /// <param name="identifier">Идентификатор чата</param>
        /// <param name="message">Сообщение</param>
        /// <param name="data">Данные</param>
        /// <returns></returns>
        Task HandleCurrentState(long identifier, object message, string data);
    }
}
