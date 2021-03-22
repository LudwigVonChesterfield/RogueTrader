using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ObjectPortrait))]
public class ShipUISlot : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector]
    public Ship ship;

    private ObjectPortrait shipPortrait;

    public void Init(Ship ship)
    {
        this.ship = ship;
        shipPortrait = GetComponent<ObjectPortrait>();
        shipPortrait.followTransform = ship.transform;
        name = ship.name + "'s UI Slot";

        transform.SetParent(UIManager.inst.shipList);
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    void OnDestroy()
    {
        if(ship != null)
        {
            ship.my_slot = null;
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Player.inst.my_camera.followTransform = ship.transform;
    }
}
