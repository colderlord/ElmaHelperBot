using System.Runtime.Serialization;

namespace EleWise.ELMA.ElmaBot.Core.Rest.Models
{
    [DataContract]
    public class StartProcessBody
    {
        [DataMember]
        public string ProcessToken { get; set; }

        [DataMember]
        public string ProcessName { get; set; }

        [DataMember]
        public object Context { get; set; }
    }
}
