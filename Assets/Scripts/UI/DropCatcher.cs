using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropCatcher : MonoBehaviour, IDropHandler
{
    public delegate void DropHandler(PointerEventData eventData);
    public event DropHandler onDrop;

    public void OnDrop(PointerEventData eventData)
    {
        onDrop?.Invoke(eventData);
    }
}
