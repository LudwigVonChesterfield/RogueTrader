using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Defines;

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
public class Selectable : MonoBehaviour
{
    public virtual SelectedType type { get { throw new System.Exception("SelectedType not implemented for Selectable."); } }
    [HideInInspector]
    public bool isSelected = false;

    void OnMouseDown()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if(Player.inst.ClickOn(this))
        {
            ClickedOn(Player.inst);
        }
    }

    public virtual void ClickedOn(Player player)
    {
        player.Select(this);
    }

    public virtual void BeforeSelect(Player player)
    {

    }

    public virtual void AfterSelect(Player player)
    {

    }

    public virtual void BeforeUnselect(Player player)
    {

    }

    public virtual void AfterUnselect(Player player)
    {

    }
}
