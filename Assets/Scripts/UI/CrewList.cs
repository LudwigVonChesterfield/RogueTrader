using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrewList : MonoBehaviour
{
    public bool expandable = false;
    public bool slotsVanish = false;

    public DropCatcher catcher;

    void Awake()
    {
        if(catcher != null)
        {
            catcher.onDrop += OnDrop;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag == null)
        {
            return;
        }

        CrewUIIcon crewIcon = eventData.pointerDrag.GetComponent<CrewUIIcon>();
        if(crewIcon == null)
        {
            return;
        }

        if(expandable)
        {
            InsertNew(crewIcon);
        }
    }

    public void InsertNew(CrewUIIcon crewIcon)
    {
        GameObject crewSlotObj = Instantiate(Game.inst.crewSlotUI_prefab, gameObject.transform);

        CrewUISlot crewSlot = crewSlotObj.GetComponent<CrewUISlot>();
        if(slotsVanish)
        {
            crewSlot.vanish_empty = slotsVanish;
        }

        crewIcon.InsertInto(crewSlot);
        crewIcon.Snap();
    }

    public void Display(ICrewContainer container)
    {
        foreach(CrewUISlot slot in container.GetCrewSlots())
        {
            slot.transform.SetParent(transform);
            slot.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    public void Hide(ICrewContainer container)
    {
        foreach(CrewUISlot slot in container.GetCrewSlots())
        {
            slot.transform.SetParent(container.GetCrewRestTransform());
        }
    }
}
