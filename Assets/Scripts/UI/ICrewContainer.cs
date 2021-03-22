using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICrewContainer
{
    List<CrewUISlot> GetCrewSlots();
    Transform GetCrewRestTransform();
    void UpdateCrewSlots();
}
