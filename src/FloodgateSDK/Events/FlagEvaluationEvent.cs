using System.Collections.Generic;

namespace FloodGate.SDK.Events
{
    public class FlagEvaluationEvent : EventBase, IEvent
    {

        

        public FlagEvaluationEvent(string sdkKey, FeatureFlagEntity flag) : base(EventTypes.FlagEvaluation, sdkKey)
        {
            EventPayload = new Dictionary<string, object>()
            {
                { "FlagKey", flag.Key }
            };
        }
    }
}
