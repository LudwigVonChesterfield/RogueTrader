using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

namespace Defines
{
    public enum RoleKeyword
    {
        Captain,
        Navigator,

        Medic,

        Mechanic,

        Foreman,
        Loader
    }
}

[RequireComponent(typeof(Ship))]
public class ShipCrewRoleHolder : IModuleHolder<Ship, ShipCrewRoleSlot, ShipCrewRole, RoleKeyword>
{
}
