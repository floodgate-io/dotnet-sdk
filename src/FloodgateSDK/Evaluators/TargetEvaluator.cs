using System;
using System.Collections.Generic;
using System.Linq;

namespace FloodGate.SDK.Evaluators
{
    public class TargetEvaluator
    {
        public static T Evaluate<T>(string key, User user, FeatureFlagEntity flag, T defaultValue, ILogger log)
        {
            bool evaluates = true;

            bool valid = false;

            List<TargetEntity> targets = flag.Targets;

            foreach (var target in targets)
            {
                evaluates = true; // reset flag for target evaluating

                foreach (var rule in target.Rules)
                {
                    var value = string.Empty;

                    var userAttributeValue = user.GetAttributeValue(rule.Attribute)?.ToLower();

                    if (userAttributeValue == null)
                    {
                        evaluates = false;
                        continue;
                    }

                    switch (rule.Comparator)
                    {
                        case Consts.COMPARATOR_EQUAL_TO:
                            valid = rule.Values
                                .ConvertAll(q => q.ToLower())
                                .Contains(userAttributeValue.ToLower());

                            log.Info($"{Consts.COMPARATOR_EQUAL_TO} {userAttributeValue} {valid}");

                            break;
                        case Consts.COMPARATOR_NOT_EQUAL_TO:
                            valid = !rule.Values
                                .ConvertAll(q => q.ToLower())
                                .Contains(userAttributeValue.ToLower());

                            log.Info($"{Consts.COMPARATOR_NOT_EQUAL_TO} {userAttributeValue} {valid}");

                            break;
                        case Consts.COMPARATOR_GREATER:
                            value = rule.Values.FirstOrDefault();

                            valid = false;

                            try
                            {
                                if (value != null && Convert.ToDouble(userAttributeValue) > Convert.ToDouble(value))
                                    valid = true;
                            }
                            catch(Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            

                            log.Info($"{Consts.COMPARATOR_GREATER} {userAttributeValue} {valid}");

                            break;
                        case Consts.COMPARATOR_GREATER_EQUAL_TO:
                            value = rule.Values.FirstOrDefault();

                            valid = false;

                            try
                            {
                                if (value != null && Convert.ToDouble(userAttributeValue) >= Convert.ToDouble(value))
                                    valid = true;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }


                            log.Info($"{Consts.COMPARATOR_GREATER_EQUAL_TO} {userAttributeValue} {valid}");

                            break;
                        case Consts.COMPARATOR_LESS:
                            value = rule.Values.FirstOrDefault();

                            valid = false;

                            try
                            {
                                if (value != null && Convert.ToDouble(userAttributeValue) < Convert.ToDouble(value))
                                    valid = true;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }


                            log.Info($"{Consts.COMPARATOR_LESS} {userAttributeValue} {valid}");

                            break;
                        case Consts.COMPARATOR_LESS_EQUAL_TO:
                            value = rule.Values.FirstOrDefault();

                            valid = false;

                            try
                            {
                                if (value != null && Convert.ToDouble(userAttributeValue) <= Convert.ToDouble(value))
                                    valid = true;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }


                            log.Info($"{Consts.COMPARATOR_LESS_EQUAL_TO} {userAttributeValue} {valid}");

                            break;
                        case Consts.COMPARATOR_CONTAINS:
                            var contains = rule.Values
                                .ConvertAll(q => q.ToLower())
                                .Where(q => userAttributeValue.Contains(q.ToString()) == true).FirstOrDefault();

                            valid = false;

                            if (contains != null)
                                valid = true;

                            log.Info($"{Consts.COMPARATOR_CONTAINS} {userAttributeValue} {valid}");

                            break;
                        case Consts.COMPARATOR_NOT_CONTAIN:
                            var notContains = rule.Values
                                .ConvertAll(q => q.ToLower())
                                .Where(q => userAttributeValue.Contains(q.ToString()) == false).FirstOrDefault();

                            valid = false;

                            if (notContains != null)
                                valid = true;

                            log.Info($"{Consts.COMPARATOR_NOT_CONTAIN} {userAttributeValue} {valid}");

                            break;

                        case Consts.COMPARATOR_ENDS_WITH:
                            var endsWith = rule.Values
                                .ConvertAll(q => q.ToLower())
                                .Where(q => userAttributeValue.EndsWith(q.ToString()) == true).FirstOrDefault();

                            valid = false;

                            if (endsWith != null)
                                valid = true;

                            log.Info($"{Consts.COMPARATOR_ENDS_WITH} {userAttributeValue} {valid}");

                            break;
                        default:
                            valid = false;

                            break;
                    }

                    // Rules are evaluated and && together
                    evaluates = evaluates && valid;
                }


                if (evaluates)
                {
                    if (target.IsRollout)
                    {
                        return RolloutEvaluator.Evaluate<T>(key, user.Id, target.Rollouts, defaultValue, log);
                    }

                    return (T)Convert.ChangeType(target.Value, typeof(T));
                }

            }

            if (flag.IsRollout) {
                // Evaluate flag rollouts
                return RolloutEvaluator.Evaluate<T>(key, user.Id, flag.Rollouts, defaultValue, log);
            }

            return (T)Convert.ChangeType(defaultValue, typeof(T));
        }
    }
}
