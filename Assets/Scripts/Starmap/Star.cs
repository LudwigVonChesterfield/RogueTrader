using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Defines;

public class Star : Selectable
{
    public override sealed SelectedType type { get { return SelectedType.Star; } }

    public Color color;
    public float size;
    public Vector3 pos;

    public List<HyperspaceCorridor> corridors = new List<HyperspaceCorridor>();

    private MeshRenderer my_renderer;

    public TextMeshPro my_text;

    void Awake()
    {
        my_renderer = GetComponent<MeshRenderer>();

        SetPos(transform.localPosition);
    }

    public override void ClickedOn(Player player)
    {
        if(player.selected_type == SelectedType.Ship)
        {
            Ship ship = (Ship) player.selected;
            if(ship.orbiting != this)
            {
                HyperspaceCorridor corridor = GetCorridor(ship.orbiting);
                if(corridor != null)
                {
                    ship.Enter(corridor, this);
                }
                return;
            }
        }

        player.Select(this);
    }

    public bool IsAdjacent(Star other)
    {
        foreach(HyperspaceCorridor corridor in corridors)
        {
            if(corridor.start == other || corridor.end == other)
            {
                return true;
            }
        }
        return false;
    }

    public HyperspaceCorridor GetCorridor(Star other)
    {
        foreach(HyperspaceCorridor corridor in corridors)
        {
            if(corridor.start == other || corridor.end == other)
            {
                return corridor;
            }
        }
        return null;
    }

    public void SetPos(Vector3 new_pos)
    {
        pos = new_pos;
        transform.localPosition = pos;

        foreach(HyperspaceCorridor corridor in corridors)
        {
            corridor.CalcLength();
        }
    }

    public void SetColor(Color new_color)
    {
        color = new_color;
        my_renderer.material.SetColor("_Color", color);
    }

    public void SetSize(float new_size)
    {
        size = new_size;
        transform.localScale = new Vector3(size, size, size);
    }
}
