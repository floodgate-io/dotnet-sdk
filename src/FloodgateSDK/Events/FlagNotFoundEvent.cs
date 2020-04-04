using System;
using System.Collections.Generic;
using System.Text;

namespace FloodGate.SDK.Events
{
    public class FlagNotFoundEvent : EventBase, IEvent
    {
        public FlagNotFoundEvent(string sdkKey, string flagKey, string evaluatedValue) : base(EventTypes.FlagNotFound, sdkKey)
        {
            EventPayload = new Dictionary<string, object>()
            {
                { "FlagKey", flagKey },
                { "Evaluation", evaluatedValue }
            };
        }
    }
}
