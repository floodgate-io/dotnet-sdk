using System;
using System.Collections.Generic;
using System.Text;

namespace FloodGate.SDK.Events
{
    public interface IEventProcessor
    {
        void AddToQueue(IEvent e);

        void ManualFlush();

        void Dispose();
    }
}
