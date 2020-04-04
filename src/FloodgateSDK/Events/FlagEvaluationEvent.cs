using System.Collections.Generic;

namespace FloodGate.SDK.Events
{
    /// <summary>
    /// Register event for each flag evaluation that occurs
    /// </summary>
    public class FlagEvaluationEvent : EventBase, IEvent
    {
        public FlagEvaluationEvent(string sdkKey, string flagKey, string evaluatedValue, User user = null) : base(EventTypes.FlagEvaluation, sdkKey)
        {
            Dictionary<string, object> userPayload = null;

            if (user != null)
            {
                List<string> customAttributeKeys = new List<string>(user.CustomAttributes.Keys);

                userPayload = new Dictionary<string, object>()
                {
                    { "Id", user.Id },
                    { "Email", user.Email },
                    { "CustomAttributes", customAttributeKeys }
                };
            }

            EventPayload = new Dictionary<string, object>()
            {
                { "FlagKey", flagKey },
                { "Evaluation", evaluatedValue },
                { "User", userPayload }
            };
        }
    }
}
