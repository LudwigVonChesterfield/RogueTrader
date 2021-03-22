using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIGroup : ISerializationCallbackReceiver
{
    public string name;
    [HideInInspector]
    public bool on = false;

    [System.Serializable]
    public class NamedGroup
    {
        [HideInInspector]
        public bool visible = false;
        public string name;
        public CanvasGroup group;
    }

    [SerializeField]
    public List<NamedGroup> elements;
    Dictionary<string, NamedGroup> group_by_name;

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        InitElements();
    }

    void InitElements()
    {
        group_by_name = new Dictionary<string, NamedGroup>();
        foreach(NamedGroup namedGroup in elements)
        {
            group_by_name[namedGroup.name] = namedGroup;
        }
    }

    public void ShowAll()
    {
        on = true;
        foreach(NamedGroup group in elements)
        {
            Show(group);
        }
    }

    public void HideAll()
    {
        on = false;
        foreach(NamedGroup group in elements)
        {
            Hide(group);
        }
    }

    private void Show(CanvasGroup cg)
    {
        cg.alpha = 1.0f;
        cg.blocksRaycasts = true;
    }

    private void Hide(CanvasGroup cg)
    {
        cg.alpha = 0.0f;
        cg.blocksRaycasts = false;
    }

    public void Show(NamedGroup group)
    {
        group.visible = true;
        Show(group.group);
    }

    public void Hide(NamedGroup group)
    {
        group.visible = false;
        Hide(group.group);
    }

    public void Show(string element)
    {
        Show(group_by_name[element]);
    }

    public void Hide(string element)
    {
        Hide(group_by_name[element]);
    }
}

public class UIManager : MonoBehaviour, ISerializationCallbackReceiver
{
    public static UIManager inst;

    [SerializeField]
    public Canvas canvas;

    [SerializeField]
    public List<UIGroup> ui_groups;
    private Dictionary<string, UIGroup> groups;

    public CrewList availableCrew;
    public CrewList shipCrew;

    public Transform shipList;

    [System.Serializable]
    public class NamedPortrait
    {
        public string name;
        public Texture portrait;
    }
    [SerializeField]
    private List<NamedPortrait> crew_portraits;

    public Dictionary<string, Texture> portraits;

    void Awake()
    {
        if(inst != null)
        {
            Destroy(this);
            return;
        }

        inst = this;

        InitPortraits();
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        groups = new Dictionary<string, UIGroup>();
        foreach(UIGroup group in ui_groups)
        {
            groups[group.name] = group;
        }
    }

    void InitPortraits()
    {
        portraits = new Dictionary<string, Texture>();
        foreach(NamedPortrait namedPortrait in crew_portraits)
        {
            portraits[namedPortrait.name] = namedPortrait.portrait;
        }
        crew_portraits = null;
    }

    void Start()
    {
        List<string> portrait_names = new List<string>
        {
            "Admiral",
            "Trader",
            "Techpriest",
            "Servitor"
        };

        foreach(string po_name in portrait_names)
        {
            CrewUIIcon crewIcon = Instantiate(Game.inst.crewIconUI_prefab, canvas.transform).GetComponent<CrewUIIcon>();
            crewIcon.SetPortraitName(po_name);
            availableCrew.InsertNew(crewIcon);
        }
    }

    public void ToggleShipUI(Ship ship)
    {
        if(groups["ShipUI"].on)
        {
            ShipUIOff(ship);
        }
        else
        {
            ShipUIOn(ship);
        }
    }

    public void ShipUIOn(Ship ship)
    {
        groups["WorldUI"].HideAll();

        UIGroup shipUI = groups["ShipUI"];

        shipCrew.Display(ship);

        if(ship.orbiting != null)
        {

            shipUI.Show("AvailableCrewList");
        }
        shipUI.Show("ShipCrewList");
        shipUI.on = true;

    }

    public void ShipUIOff(Ship ship)
    {
        UIGroup shipUI = groups["ShipUI"];

        shipCrew.Hide(ship);

        shipUI.Hide("AvailableCrewList");
        shipUI.Hide("ShipCrewList");
        shipUI.on = false;

        groups["WorldUI"].ShowAll();
    }
}
