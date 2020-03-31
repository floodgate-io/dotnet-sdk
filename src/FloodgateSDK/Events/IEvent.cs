using System;
using System.Collections.Generic;

namespace FloodGate.SDK.Events
{
    public interface IEvent
    {
        string SdkKey { get; }

        Guid ClientEventId { get; }

        EventTypes EventType { get; }

        int EventTime { get; }

        Dictionary<string, object> EventPayload { get; }
    }
}
