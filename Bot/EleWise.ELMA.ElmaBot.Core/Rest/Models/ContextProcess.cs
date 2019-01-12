using System.Runtime.Serialization;

namespace EleWise.ELMA.ElmaBot.Core.Rest.Models
{
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
}
