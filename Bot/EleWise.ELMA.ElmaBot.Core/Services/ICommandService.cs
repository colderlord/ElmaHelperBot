using EleWise.ELMA.ElmaBot.Core.Models;

namespace EleWise.ELMA.ElmaBot.Core.Services
{
    /// <summary>
    /// Сервис для работы с командами бота
    /// </summary>
    public interface ICommandService
    {
        /// <summary>
        /// Получить команду по тексту
        /// </summary>
        /// <param name="text">Текст</param>
        /// <returns>Команда</returns>
        ICommand GetCommandFromText(string text);
    }
}
