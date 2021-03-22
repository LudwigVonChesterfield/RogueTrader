using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITabGroup<T> : UIStructure<T>
{
    public List<UITabButton<T>> tabButtons;

    public void Subscribe(UITabButton<T> button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<UITabButton<T>>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(UITabButton<T> button, PointerEventData eventData)
    {

    }

    public void OnTabExit(UITabButton<T> button, PointerEventData eventData)
    {

    }

    public void OnTabSelected(UITabButton<T> button, PointerEventData eventData)
    {

    }
}
