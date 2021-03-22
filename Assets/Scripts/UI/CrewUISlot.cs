using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrewUISlot : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    public CrewUIIcon filled;

    private Canvas canvas;
    private RectTransform rectTransform;

    public RectTransform iconContainerTransform;

    public bool vanish_empty;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        // iconContainerTransform.gameObject.SetActive(false);
    }

    void Start()
    {
        canvas = UIManager.inst.canvas;
    }

    void OnDestroy()
    {
        if(filled != null)
        {
            filled.slot = null;
            Destroy(filled);
        }

        canvas = null;
        rectTransform = null;
        iconContainerTransform = null;

        filled = null;
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

        crewIcon.InsertInto(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnInserted(CrewUIIcon crewIcon)
    {
        filled = crewIcon;
        // iconContainerTransform.gameObject.SetActive(true);
    }

    public void OnLeft(CrewUIIcon crewIcon)
    {
        // iconContainerTransform.gameObject.SetActive(false);
        filled = null;
        if(vanish_empty)
        {
            Destroy(gameObject);
        }
    }
}
