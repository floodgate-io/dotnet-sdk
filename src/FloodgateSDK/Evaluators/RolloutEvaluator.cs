using System;
using System.Collections.Generic;

namespace FloodGate.SDK.Evaluators
{
    public class RolloutEvaluator
    {
        /// <summary>
        /// Check to see if a user falls into a given percentate rollout group
        /// </summary>
        /// <returns>True if user evaluates</returns>
        public static T Evaluate<T>(string key, string userId, List<RolloutEntity> rollouts, T defaultValue, ILogger log)
        {
            var scale = EvaluatorHelper.GetScale(key, userId);

            var rolloutLowerLimit = 0;
            var rolloutUpperLimit = 0;
            var rolloutValue = defaultValue;

            foreach (var rollout in rollouts)
            {
                rolloutUpperLimit += rollout.Percentage;

                if (scale > rolloutLowerLimit && scale <= rolloutUpperLimit)
                {
                    log.Info($"{userId} ({scale}) = {rollout.Value}");

                    rolloutValue = (T)Convert.ChangeType(rollout.Value, typeof(T));
                    break;
                }

                rolloutLowerLimit += rollout.Percentage;
            }

            return rolloutValue;
        }
    }
}
