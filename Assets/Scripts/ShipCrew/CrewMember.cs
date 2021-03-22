using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember : MonoBehaviour
{
    public string portaitName;

    public CrewUIIcon GenIcon(CrewUISlot slot)
    {
        CrewUIIcon crewIcon = Instantiate(Game.inst.crewIconUI_prefab, slot.gameObject.transform).GetComponent<CrewUIIcon>();
        crewIcon.member = this;
        crewIcon.SetPortraitName(portaitName);
        return crewIcon;
    }
}
