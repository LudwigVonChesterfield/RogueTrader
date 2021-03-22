using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStructures.Stats
{
    public enum StatModifierType
    {
        Additive,
        Multiplicative
    }

    public enum StatModifierSubject
    {
        Base,
        Value
    }

    public class StatModifier
    {
        public StatModifierType applyType;
        public StatModifierSubject applySubject;
        public float value = 0.0f;

        public T Inverse<T>() where T : StatModifier, new()
        {
            T inverse = new T();
            inverse.applyType = applyType;
            inverse.applySubject = applySubject;

            switch(applyType)
            {
                case StatModifierType.Additive:
                {
                    inverse.value = value * -1.0f;
                    break;
                }
                case StatModifierType.Multiplicative:
                {
                    inverse.value = 1 / value;
                    break;
                }
            }

            return inverse;
        }
    }
}
