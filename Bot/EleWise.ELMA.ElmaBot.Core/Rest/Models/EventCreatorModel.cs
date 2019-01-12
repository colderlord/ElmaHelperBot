using System.Runtime.Serialization;

namespace EleWise.ELMA.ElmaBot.Core.Rest.Models
{
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
