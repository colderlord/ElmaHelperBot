using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using EleWise.ELMA.Services.Public;
using EleWise.ELMA.Web.Service;

namespace EleWise.ELMA.ELMAHelperBot.Services
{
    [ServiceContract(Namespace = APIRouteProvider.ApiServiceNamespaceRoot)]
    [WsdlDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.BotWebApiServiceDescription))]
    [FaultContractBehavior(typeof(PublicServiceException))]
    public interface IBotService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/UpdateUser?chatId={chatId}&userId={userId}")]
        [WsdlDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.CheckLoginDescription))]
        [return: WsdlParamOrReturnDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.CheckLoginDescription))]
        void UpdateUser([WsdlParamOrReturnDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.LoginDescription))] string chatId, string userId);

        [OperationContract]
        [WebGet(UriTemplate = "/GetProcessContext?headerid={headerid}")]
        [WsdlDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.CheckLoginDescription))]
        [return: WsdlParamOrReturnDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.CheckLoginDescription))]
        List<ContextProcess> GetProcessContext([WsdlParamOrReturnDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.LoginDescription))] long headerid);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/EventCreate")]
        [WsdlDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.CheckLoginDescription))]
        [return: WsdlParamOrReturnDocumentation(typeof(__ITestWebApiServiceResources), nameof(__ITestWebApiServiceResources.CheckLoginDescription))]
        bool EventCreate(EventCreatorModel model);
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
