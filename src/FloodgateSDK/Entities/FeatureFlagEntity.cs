using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FloodGate.SDK
{
    [DataContract]
    public class FeatureFlagEntity
    {
#pragma warning disable CS0649
        [DataMember (Name = "id")]
        internal string Id;

#pragma warning disable CS0649
        [DataMember(Name = "name")]
        internal string Name;

#pragma warning disable CS0649
        [DataMember(Name = "key")]
        internal string Key;

#pragma warning disable CS0649
        [DataMember(Name = "state")]
        internal bool State;

#pragma warning disable CS0649
        [DataMember(Name = "value")]
        internal string Value;

#pragma warning disable CS0649
        [DataMember(Name = "is_rollout")]
        internal bool IsRollout;

#pragma warning disable CS0649
        [DataMember(Name = "rollouts")]
        internal List<RolloutEntity> Rollouts;

#pragma warning disable CS0649
        [DataMember (Name = "is_targeting_enabled")]
        internal bool IsTargetingEnabled;

#pragma warning disable CS0649
        [DataMember (Name = "targets")]
        internal List<TargetEntity> Targets;
    }
}
