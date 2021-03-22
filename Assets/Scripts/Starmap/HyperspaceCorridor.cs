using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class HyperspaceCorridor : Selectable
{
    public override sealed SelectedType type { get { return SelectedType.HyperspaceCorridor; } }

    public Color color;
    public float thickness;
    public float currentSpeed;
    private float length;

    public Star start;
    public Star end;

    private MeshRenderer my_renderer;

    void Awake()
    {
        my_renderer = GetComponent<MeshRenderer>();
    }

    public void CalcLength()
    {
        length = Vector3.Distance(start.pos, end.pos);
    }

    public void SetStartEnd(Star new_start, Star new_end)
    {
        start = new_start;
        end = new_end;

        start.corridors.Add(this);
        end.corridors.Add(this);

        CalcLength();
    }

    public void SetColor(Color new_color)
    {
        color = new_color;
        my_renderer.material.SetColor("_Color", color);
    }

    public void SetThickness(float new_thickness)
    {
        thickness = new_thickness;
        // 0.5f since Scale scales up into both ways
        transform.localScale = new Vector3(thickness, length * 0.5f, thickness);
    }

    public void SetCurrentSpeed(float new_speed)
    {
        currentSpeed = new_speed;
        SetThickness(0.1f + currentSpeed * 0.1f);
    }

    public void BeforeEnter(Ship ship)
    {

    }

    public void AfterEnter(Ship ship)
    {

    }

    public void BeforeExit(Ship ship)
    {

    }

    public void AfterExit(Ship ship)
    {

    }
}
