using System.Collections.Generic;

namespace FloodGate.SDK.Events
{
    public class SetUserEvent : EventBase, IEvent
    {
        public SetUserEvent(string sdkKey, User user) : base(EventTypes.SetUser, sdkKey)
        {
            List<string> keyList = new List<string>(user.CustomAttributes.Keys);

            EventPayload = new Dictionary<string, object>()
            {
                { "Id", user.Id },
                { "CustomAttributes", keyList }
            };
        }
    }
}
