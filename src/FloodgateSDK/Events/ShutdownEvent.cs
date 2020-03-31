using System;
using System.Collections.Generic;
using System.Text;

namespace FloodGate.SDK.Events
{
    public class ShutdownEvent : EventBase, IEvent
    {
        public ShutdownEvent() : base(EventTypes.Shutdown)
        {
        
        }
    }
}
