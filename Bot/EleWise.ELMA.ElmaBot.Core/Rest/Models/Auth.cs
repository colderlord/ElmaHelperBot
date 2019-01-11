using System.Runtime.Serialization;

namespace EleWise.ELMA.ElmaBot.Core.Rest.Models
{
    /// <summary>
    /// Модель авторизации
    /// </summary>
    [DataContract]
    public sealed class Auth
    {
        /// <summary>
        /// Токен авторизации
        /// </summary>
        [DataMember]
        public string AuthToken { get; set; }

        /// <summary>
        /// Идентификатор текущего пользователя
        /// </summary>
        [DataMember]
        public string CurrentUserId { get; set; }

        /// <summary>
        /// Токен сессии
        /// </summary>
        [DataMember]
        public string SessionToken { get; set; }
    }
}
