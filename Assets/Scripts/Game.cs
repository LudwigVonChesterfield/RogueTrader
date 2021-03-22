using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game inst;

    public GameObject star_prefab;
    public GameObject corridor_prefab;

    public GameObject crewIconUI_prefab;
    public GameObject crewSlotUI_prefab;

    public GameObject shipSlotUI_prefab;

    public StarGraph map;
    public GameObject map_obj;

    public GameObject background;

    public GameObject gameWorld;

    void Awake()
    {
        if(inst != null)
        {
            Destroy(this);
            return;
        }

        inst = this;

        map = new StarGraph();
        map.Init(map_obj, background);
    }
}
