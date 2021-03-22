using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CrewUIIcon : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private string portraitName;

    public CrewMember member;

    public CrewUISlot slot;

    private Canvas canvas;

    private RawImage image;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void OnEnable()
    {
        if(slot != null)
        {
            rectTransform.anchoredPosition = slot.gameObject.GetComponent<RectTransform>().anchoredPosition;
        }
    }

    void Awake()
    {
        image = GetComponent<RawImage>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        canvas = UIManager.inst.canvas;
    }

    void OnDestroy()
    {
        if(slot != null)
        {
            slot.filled = null;
        }

        canvas = null;
        image = null;
        canvasGroup = null;
        rectTransform = null;

        slot = null;
    }

    public void SetPortraitName(string new_name)
    {
        portraitName = new_name;
        image.texture = UIManager.inst.portraits[portraitName];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTransform.SetParent(canvas.transform);
        canvasGroup.ignoreParentGroups = true;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.56f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.ignoreParentGroups = false;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;
        Snap();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

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

        CrewUISlot cur_slot = slot;
        bool cur_slot_vanish = cur_slot.vanish_empty;
        cur_slot.vanish_empty = false;
    
        CrewUISlot next_slot = crewIcon.slot;
        bool next_slot_vanish = next_slot.vanish_empty;
        next_slot.vanish_empty = false;

        LeaveSlot();
        crewIcon.InsertInto(cur_slot);
        InsertInto(next_slot);
        Snap();

        cur_slot.vanish_empty = cur_slot_vanish;
        next_slot.vanish_empty = next_slot_vanish;
    }

    public void InsertInto(CrewUISlot new_slot)
    {
        if(slot != null)
        {
            slot.OnLeft(this);
        }

        slot = new_slot;
        slot.OnInserted(this);
    }

    public void LeaveSlot()
    {
        if(slot != null)
        {
            slot.OnLeft(this);
            slot = null;
        }
    }

    public void Snap()
    {
        if(slot == null)
        {
            throw new System.Exception("CrewUIIcon Error: Slot absent after dragging.");
        }

        rectTransform.SetParent(slot.iconContainerTransform);
        rectTransform.anchoredPosition = slot.iconContainerTransform.anchoredPosition;
    }
}
