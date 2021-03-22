using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RawImage))]
public class UITabButton<T> : UIStructure<T>, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public UITabGroup<T> tabGroup;

    private RawImage background;

    protected override void OnAwake()
    {
        background = GetComponent<RawImage>();
        tabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this, eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this, eventData); 
    }
}
