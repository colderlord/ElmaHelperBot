using System;
using System.Linq.Expressions;
using System.ServiceModel;
using System.ServiceModel.Activation;
using EleWise.ELMA.ComponentModel;
using EleWise.ELMA.Logging;
using EleWise.ELMA.Model.Attributes;
using EleWise.ELMA.Security.Managers;
using EleWise.ELMA.Web.Service;

namespace EleWise.ELMA.ELMAHelperBot.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, MaxItemsInObjectGraph = int.MaxValue)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceKnownType("GetGlobalKnownTypes", typeof(ServiceKnownTypeHelper))]
    [Component]
    [Uid(GuidS)]
    public class BotService : IBotService, IPublicAPIWebService
    {
        /// <summary>
        /// Уникальный идентификатор сервиса (строковое представление)
        /// </summary>
        public const string GuidS = "{B94DF052-7F48-47AB-800A-0B7F6F50126E}";

        /// <summary>
        /// Уникальный идентификатор сервиса
        /// </summary>
        public static Guid Guid = new Guid(GuidS);

        /// <summary>
        /// Возвращает свойство логгера
        /// </summary>
        public ILogger Logger { get { return Logging.Logger.Log; } }

        /// <inheritdoc />
        public void UpdateUser(string chatId, string userId)
        {
            var id = long.Parse(userId);

            var user = UserManager.Instance.LoadOrNull(id);
            user.JabberID = chatId;
            user.Save();
        }

        private static void LogServiceError(Expression<Func<BotService, object>> method, string message)
        {
            Logging.Logger.Log.Error(SR.T("В сервисе \"{0}\" в методе \"{1}\" произошла ошибка: {2}", typeof(BotService), method, message));
        }
    }
}
