using System;
using System.Collections.Generic;
using System.Text;

namespace FloodGate.SDK.Events
{
    public abstract class EventBase
    {
        public string SdkKey { get; }

        /// <summary>
        /// Unique id of the event
        /// </summary>
        public Guid ClientEventId { get; } = Guid.NewGuid();

        /// <summary>
        /// The type of event being generated
        /// </summary>
        public EventTypes EventType { get; }

        /// <summary>
        /// The date/time of the event
        /// </summary>
        public int EventTime { get; }

        /// <summary>
        /// The actual event data
        /// </summary>
        public Dictionary<string, object> EventPayload { get; internal set;  }

        public EventBase(EventTypes eventType, string sdkKey)
        {
            EventType = eventType;

            SdkKey = sdkKey;

            EventTime = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public EventBase(EventTypes eventType)
        {
            EventType = eventType;

            EventTime = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
