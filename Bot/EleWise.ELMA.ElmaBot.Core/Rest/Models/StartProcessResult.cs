using System.Runtime.Serialization;

namespace EleWise.ELMA.ElmaBot.Core.Rest.Models
{
    [DataContract]
    public class StartProcessResult
    {
        [DataMember]
        public bool Result { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string ExecutionToken { get; set; }

        [DataMember]
        public long NextTaskId { get; set; }
    }
}
