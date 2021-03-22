using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;
using DataStructures.Stats;

[CreateAssetMenu(menuName = "Ship/Ship Module")]
public class ShipModule : IModule<Ship, SlotKeyword>
{
    public List<ShipStatMod> stat_modifiers;
    public List<ShipCrewRoleSlot> role_slot_unlocks;

    public override void Install(Ship ship)
    {
        foreach(ShipStatMod mod in stat_modifiers)
        {
            ship.stats.AddModifier(mod);
        }

        foreach(ShipCrewRoleSlot role_slot in role_slot_unlocks)
        {
            ship.roles.AddSlot(role_slot);
        }
    }

    public override void Uninstall(Ship ship)
    {
        foreach(ShipStatMod mod in stat_modifiers)
        {
            ship.stats.AddModifier(mod.Inverse<ShipStatMod>());
        }

        foreach(ShipCrewRoleSlot role_slot in role_slot_unlocks)
        {
            ship.roles.RemoveSlot(role_slot);
        }
    }
}
