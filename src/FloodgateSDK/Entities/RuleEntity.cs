using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FloodGate.SDK
{
    [DataContract]
    public class RuleEntity
    {
#pragma warning disable CS0649
        [DataMember(Name = "id")]
        internal string Id;

#pragma warning disable CS0649
        [DataMember(Name = "attribute")]
        internal string Attribute;

#pragma warning disable CS0649
        [DataMember(Name = "comparator")]
        internal int Comparator;

#pragma warning disable CS0649
        [DataMember(Name = "values")]
        internal List<string> Values;
    }
}
