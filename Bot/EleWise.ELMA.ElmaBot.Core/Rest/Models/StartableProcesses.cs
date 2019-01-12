using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EleWise.ELMA.ElmaBot.Core.Rest.Models
{
    [DataContract]
    public class StartableProcesses
    {
        [DataMember]
        public List<Groups> Groups { get; set; }

        [DataMember]
        public List<Processes> Processes { get; set; }
    }

    [DataContract]
    public class Processes
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class Groups
    {

    }

    [DataContract]
    public class StartProcessForm
    {
        [DataMember]
        public object Context { get; set; }
    }
}
