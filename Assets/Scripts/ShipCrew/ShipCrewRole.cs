using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

[CreateAssetMenu(menuName = "Crew/Crew Role")]
public class ShipCrewRole : IModule<Ship, RoleKeyword>
{
    public override void Install(Ship ship)
    {
    }

    public override void Uninstall(Ship ship)
    {
    }
}
