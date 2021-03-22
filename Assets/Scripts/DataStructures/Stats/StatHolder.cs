using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStructures.Stats
{
    public class StatHolder<TEnum> where TEnum : System.Enum
    {
        private Dictionary<TEnum, StatBasedValue> stats = new Dictionary<TEnum, StatBasedValue>();

        public StatHolder(Dictionary<TEnum, float> def_values)
        {
            foreach(KeyValuePair<TEnum, float> stat_value in def_values)
            {
                stats[stat_value.Key] = new StatBasedValue(stat_value.Value);
            }
        }

        public void AddModifier(TEnum type, StatModifier mod)
        {
            stats[type].AddModifier(mod);
        }

        public StatValue Get(TEnum type)
        {
            return stats[type].value;
        }

        public float GetValue(TEnum type)
        {
            return stats[type].value.value;
        }
    }
}
