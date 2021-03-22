using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

namespace Defines
{
    public enum SelectedType
    {
        None,
        Ship,
        Star,
        HyperspaceCorridor,
        Background
    }
}

public class Player : MonoBehaviour
{
    public static Player inst;

    public CameraController my_camera;

    [HideInInspector]
    public Selectable selected;
    [HideInInspector]
    public SelectedType selected_type = SelectedType.None;

    void Awake()
    {
        if(inst != null)
        {
            Destroy(this);
            return;
        }

        inst = this;
    }

    // Return false to prevent click on sel.
    public bool ClickOn(Selectable sel)
    {
        return true;
    }

    public void Select(Selectable target)
    {
        if(selected != null)
        {
            Unselect();
        }

        target.BeforeSelect(this);

        selected = target;
        selected_type = target.type;
        selected.isSelected = true;

        target.AfterSelect(this);
    }

    public void Unselect()
    {
        if(selected == null)
        {
            return;
        }

        selected.BeforeUnselect(this);
        Selectable sel = selected;

        selected.isSelected = false;
        selected = null;
        selected_type = SelectedType.None;

        sel.AfterUnselect(this);
    }
}
