using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FloodGate.SDK
{
    [DataContract]
    public class RolloutEntity
    {
//#pragma warning disable CS0649
//        [DataMember(Name = "id")]
//        internal string Id;

#pragma warning disable CS0649
        [DataMember(Name = "value")]
        internal string Value;

#pragma warning disable CS0649
        [DataMember(Name = "percentage")]
        internal int Percentage;
    }
}