using System.ServiceModel;
using System.ServiceModel.Web;
using EleWise.ELMA.API.Models;
using EleWise.ELMA.Services.Public;
using EleWise.ELMA.Web.Service;

namespace EleWise.ELMA.ELMAHelperBot.Services
{
    [ServiceContract(Namespace = APIRouteProvider.ApiServiceNamespaceRoot)]
    [WsdlDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.BotWebApiServiceDescription))]
    [FaultContractBehavior(typeof(PublicServiceException))]
    public interface IBotWebApiService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/CheckLogin?login={login}")]
        [WsdlDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.CheckLoginDescription))]
        [return: WsdlParamOrReturnDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.CheckLoginDescription))]
        AuthResponse CheckLogin(
            [WsdlParamOrReturnDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.LoginDescription))] string login);

        [OperationContract]
        [WebGet(UriTemplate = "/BotAuth?something={something}&userName={userName}")]
        [WsdlDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.CheckLoginDescription))]
        [return: WsdlParamOrReturnDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.CheckLoginDescription))]
        AuthResponse BotAuth(
            [WsdlParamOrReturnDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.LoginDescription))] string login,
            [WsdlParamOrReturnDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.UserNameDescription))] string userName,
            [WsdlParamOrReturnDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.PasswordDescription))] string password);
    }

    public class __ITestWebApiServiceResources
    {
        public static string BotWebApiServiceDescription { get { return SR.T("Сервис бота"); } }
        public static string CheckLoginDescription { get { return SR.T("Проверка авторизации"); } }
        public static string LoginDescription { get { return SR.T("Логин в боте"); } }
        public static string UserNameDescription { get { return SR.T("Логин в ELMA"); } }
        public static string PasswordDescription { get { return SR.T("Пароль"); } }
    }
}
