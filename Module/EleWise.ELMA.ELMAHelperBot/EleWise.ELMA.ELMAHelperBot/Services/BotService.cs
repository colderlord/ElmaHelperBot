using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using EleWise.ELMA.Calendar.Models;
using EleWise.ELMA.ComponentModel;
using EleWise.ELMA.Logging;
using EleWise.ELMA.Model.Attributes;
using EleWise.ELMA.Model.Metadata;
using EleWise.ELMA.Model.Services;
using EleWise.ELMA.Security.Managers;
using EleWise.ELMA.Security.Models;
using EleWise.ELMA.Security.Services;
using EleWise.ELMA.Services;
using EleWise.ELMA.Web.Service;
using EleWise.ELMA.Workflow.Managers;

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

        public List<ContextProcess> GetProcessContext(long headerid)
        {
            var header = ProcessHeaderManager.Instance.LoadOrNull(headerid);
            if (header == null)
            {
                return new List<ContextProcess>();
            }
            var published = header.Published;
            if (published == null)
            {
                return new List<ContextProcess>();
            }

            var runtimeService = Locator.GetServiceNotNull<IMetadataRuntimeService>();
            var contextMetadata = runtimeService.GetMetadata(published.Context.Uid) as EntityMetadata;
            if (contextMetadata == null)
            {
                return new List<ContextProcess>();
            }

            return contextMetadata.Properties.Where(p => !p.IsSystem && p.SubTypeUid == Guid.Empty && p.Name != "Bot").Select(p =>
            {
                var descriptor = runtimeService.GetTypeDescriptor(p.TypeUid, p.SubTypeUid);
                return new ContextProcess {
                    Name = p.Name,
                    Type = descriptor.Name,
                    DisplayName = p.DisplayName
                };
            }).ToList();
        }

        public bool EventCreate(EventCreatorModel model)
        {
            var date = model.Date;
            var timeFrom = model.TimeFrom;
            var timeTo = model.TimeTo;
            var subject = model.Subject;
            var description = model.Description;
            DateTime DateTimeFrom;
            DateTime DateTimeTo;
            if (!parseDateTime(date, timeFrom, timeTo, out DateTimeFrom, out DateTimeTo))
            {
                return false;
            }

            try
            {
                var curUser = AuthenticationService.GetCurrentUser<IUser>();

                var calEvent = InterfaceActivator.Create<ICalendarEvent>();
                var calEventUser = InterfaceActivator.Create<ICalendarEventUser>();
                calEventUser.User = curUser;
                calEventUser.Save();
                calEvent.EventUsers.Add(calEventUser);

                calEvent.StartDate = DateTimeFrom;
                calEvent.EndDate = DateTimeTo;
                calEvent.Subject = subject;
                calEvent.Description = description;
                calEvent.CreationDate = DateTime.Now;
                calEvent.CreationAuthor = curUser;
                calEvent.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool parseDateTime(string Date, string TimeFrom, string TimeTo, out DateTime DateTimeFrom, out DateTime DateTimeTo)
        {
            DateTimeFrom = DateTime.Today;
            DateTimeTo = DateTime.Today;

            DateTime CurDate = DateTime.Now.Date;
            int Day = 0;
            int Month = 0;
            int Year = CurDate.Year;
            int pointPos = Date.IndexOf(".");
            int timeFromPointsPos = TimeFrom.IndexOf(":");
            int timeToPointsPos = TimeTo.IndexOf(":");
            int HourFrom;
            int HourTo;
            int MinuteFrom;
            int MinuteTo;
            try
            {
                if (timeFromPointsPos < 0 || timeToPointsPos < 0)
                {
                    return false;
                }
                if (pointPos < 0)
                {
                    if (!int.TryParse(Date, out Day))
                    {
                        return false;
                    }
                    int CurDay = CurDate.Day;
                    Month = CurDate.Month;
                    if (Day < CurDay)
                        Month++;
                }
                else
                {
                    if (!int.TryParse(Date.Substring(0, pointPos), out Day))
                    {
                        return false;
                    }
                    if (!int.TryParse(Date.Substring(pointPos + 1), out Month))
                    {
                        return false;
                    }
                }

                DateTime DateNew = new DateTime(Year, Month, Day);
                if (DateNew < CurDate)
                {
                    DateNew.AddYears(1);
                    Year++;
                }

                if (!int.TryParse(TimeFrom.Substring(0, timeFromPointsPos), out HourFrom))
                {
                    return false;
                }
                if (!int.TryParse(TimeFrom.Substring(timeFromPointsPos + 1), out MinuteFrom))
                {
                    return false;
                }
                if (!int.TryParse(TimeTo.Substring(0, timeToPointsPos), out HourTo))
                {
                    return false;
                }
                if (!int.TryParse(TimeTo.Substring(timeToPointsPos + 1), out MinuteTo))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            DateTimeFrom = new DateTime(Year, Month, Day, HourFrom, MinuteFrom, 0);
            DateTimeTo = new DateTime(Year, Month, Day, HourTo, MinuteTo, 0);
            return true;
        }

        private static void LogServiceError(Expression<Func<BotService, object>> method, string message)
        {
            Logging.Logger.Log.Error(SR.T("В сервисе \"{0}\" в методе \"{1}\" произошла ошибка: {2}", typeof(BotService), method, message));
        }
    }

    [DataContract]
    public class ContextProcess
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string DisplayName { get; set; }
    }

    [DataContract]
    public class EventCreatorModel
    {
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string TimeFrom { get; set; }
        [DataMember]
        public string TimeTo { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
