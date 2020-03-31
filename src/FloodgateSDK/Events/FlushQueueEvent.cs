using System;
using System.Collections.Generic;
using System.Text;

namespace FloodGate.SDK.Events
{
    public class FlushQueueEvent : EventBase, IEvent
    {
        private string Reason { get; }

        public FlushQueueEvent(string reason) : base(EventTypes.FlushQueue)
        {
            Reason = reason;
        }
    }
}
