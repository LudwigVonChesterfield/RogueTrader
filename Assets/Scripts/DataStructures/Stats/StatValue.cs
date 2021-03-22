using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructures.Stats;

namespace DataStructures.Stats
{
    public class StatValue
    {
        public delegate void StatHandler();
        public event StatHandler onChange;

        public float value = 0.0f;

        float def_value = 0.0f;

        float mult_total = 1.0f;
        float add_total = 0.0f;

        public StatValue(float new_value)
        {
            value = new_value;
            def_value = value;
        }

        public void AddModifier(StatModifier mod)
        {
            switch(mod.applyType)
            {
                case StatModifierType.Additive:
                {
                    add_total += mod.value;
                    break;
                }
                case StatModifierType.Multiplicative:
                {
                    mult_total *= mod.value;
                    break;
                }
            }
            UpdateValue();
        }

        public void SetDefault(float new_value)
        {
            def_value = new_value;
            UpdateValue();
        }

        public void UpdateValue()
        {
            value = def_value * mult_total + add_total;
            onChange?.Invoke();
        }
    }

    public class StatBasedValue
    {
        public StatValue value;
        StatValue base_val;

        public StatBasedValue(float def_value)
        {
            value = new StatValue(def_value);
            base_val = new StatValue(def_value);
        }

        public void AddModifier(StatModifier mod)
        {
            switch(mod.applySubject)
            {
                case StatModifierSubject.Base:
                {
                    base_val.AddModifier(mod);
                    value.SetDefault(base_val.value);
                    break;
                }
                case StatModifierSubject.Value:
                {
                    value.AddModifier(mod);
                    break;
                }
            }
        }
    }
}
