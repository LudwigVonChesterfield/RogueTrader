using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

namespace Defines
{
    public enum SlotKeyword
    {
        Giant,
        Big,
        Medium,
        Small,
        Tiny,

        Cargo,
        Engine,
        Bridge,
        Quarters,
        Medbay,
        Weapon,
        Atmosphere,
        Forcefield
    }
}

[RequireComponent(typeof(Ship))]
public class ShipModuleHolder : IModuleHolder<Ship, ShipModuleSlot, ShipModule, SlotKeyword>
{
}
