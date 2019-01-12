using System;
using System.Linq.Expressions;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using EleWise.ELMA.API.Models;
using EleWise.ELMA.ComponentModel;
using EleWise.ELMA.Logging;
using EleWise.ELMA.Model.Attributes;
using EleWise.ELMA.Security;
using EleWise.ELMA.Services;
using EleWise.ELMA.Services.Public;
using EleWise.ELMA.Web.Service;

namespace EleWise.ELMA.ELMAHelperBot.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, MaxItemsInObjectGraph = int.MaxValue)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceKnownType("GetGlobalKnownTypes", typeof(ServiceKnownTypeHelper))]
    [Component]
    [Uid(GuidS)]
    public class TestWebApiService : IBotWebApiService, IPublicAPIWebService
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
        public AuthResponse CheckLogin(string login)
        {
            // Проверить в пользователях есть ли в JabberId такой login, если нет - вернуть null
            return null;
        }

        public AuthResponse BotAuth(string login, string userName, string password)
        {
            var membership = Locator.GetServiceNotNull<IMembershipService>();
            var user = membership.ValidateUser(userName, password);
            if (user == null)
            {
                LogServiceError(s => s.BotAuth(login, userName, password), SR.T("Ошибка авторизации по логину паролю"));
                throw PublicServiceException.CreateWebFault(SR.T("Ошибка авторизации"), (int)HttpStatusCode.Unauthorized);
            }

            return null;
        }

        private static void LogServiceError(Expression<Func<TestWebApiService, object>> method, string message)
        {
            Logging.Logger.Log.Error(SR.T("В сервисе \"{0}\" в методе \"{1}\" произошла ошибка: {2}", typeof(TestWebApiService), method, message));
        }
    }
}
