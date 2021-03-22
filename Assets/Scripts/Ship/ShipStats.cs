using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;
using DataStructures.Stats;

namespace Defines
{
    public enum ShipStatType
    {
        MoveSpeed,
        CrewSeats
    }
}

[System.Serializable]
public class ShipStatMod : StatModifier
{
    public ShipStatType statType;
}

public class ShipStats : StatHolder<ShipStatType>
{
    public ShipStats(Dictionary<ShipStatType, float> def_values) : base(def_values)
    {
    }

    public void AddModifier(ShipStatMod mod)
    {
        AddModifier(mod.statType, mod);
    }
}
