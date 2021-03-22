using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

[RequireComponent(typeof(MeshRenderer), typeof(Orbit), typeof(ShipModuleHolder))]
public class Ship : Selectable, ICrewContainer
{
    public override sealed SelectedType type { get { return SelectedType.Ship; } }

    public ShipPreset preset;

    public enum ShipState
    {
        Idle,
        Flying
    }

    public float orbitDistance = 0.5f;
    public float hoverDistance = 2.0f;

    public ShipStats stats;

    public float moveSpeed
    {
        get
        {
            return stats.GetValue(ShipStatType.MoveSpeed);
        }
    }

    public float crewSeats
    {
        get
        {
            return stats.GetValue(ShipStatType.CrewSeats);
        }
    }

    public List<CrewUISlot> availableCrew;

    [HideInInspector]
    // Is calculated based on moveSpeed and currentSpeed when entering into hyperspace corridor.
    public float curSpeed = 0.0f;

    [HideInInspector]
    public ShipState state = ShipState.Idle;

    [HideInInspector]
    public Star orbiting;

    [HideInInspector]
    public Star target;
    [HideInInspector]
    public Vector3 target_pos;

    [HideInInspector]
    public HyperspaceCorridor traveling_through;

    [HideInInspector]
    public ShipModuleHolder modules;

    [HideInInspector]
    public ShipCrewRoleHolder roles;

    public ShipUISlot my_slot;

    private MeshRenderer my_renderer;
    private Orbit my_orbit;

    void Awake()
    {
        my_renderer = GetComponent<MeshRenderer>();
        modules = GetComponent<ShipModuleHolder>();
        roles = GetComponent<ShipCrewRoleHolder>();
        my_orbit = GetComponent<Orbit>();

        stats = new ShipStats(new Dictionary<ShipStatType, float>
        {
            {ShipStatType.MoveSpeed, 3.0f},
            {ShipStatType.CrewSeats, 0.0f}
        });
        stats.Get(ShipStatType.MoveSpeed).onChange += UpdateSpeed;
        stats.Get(ShipStatType.CrewSeats).onChange += UpdateCrewSlots;
    }

    void Start()
    {
        modules.Init(preset);

        my_orbit.moveSpeed = moveSpeed;

        SpawnIn();
    }

    void OnDestroy()
    {
        if(traveling_through != null)
        {
            Exit();
        }

        if(my_slot != null)
        {
            my_slot.ship = null;
            Destroy(my_slot);
            my_slot = null;
        }

        UnsetOrbiting();
        UnsetTarget();

        my_renderer = null;
        my_orbit = null;
    }

    void Update()
    {
        if(state == ShipState.Flying)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target_pos, curSpeed * Time.deltaTime);
            if(Vector3.Distance(transform.localPosition, target_pos) < moveSpeed * Time.deltaTime)
            {
                Exit();
            }
        }
    }

    void SpawnIn()
    {
        SetOrbiting(Game.inst.map.stars[Random.Range(0, Game.inst.map.stars.Count)]);
        Player.inst.my_camera.followTransform = transform;

        my_slot = Instantiate(Game.inst.shipSlotUI_prefab, UIManager.inst.shipList).GetComponent<ShipUISlot>();
        my_slot.Init(this);
    }

    public override void ClickedOn(Player player)
    {
        if(player.selected == this)
        {
            UIManager.inst.ToggleShipUI(this);
            player.my_camera.followTransform = transform;
            return;
        }

        player.Select(this);
    }

    public void UpdateSpeed()
    {
        // Directional hyperspace currents.
        // float currentCoeff = target == traveling_through.start ? -1.0 : 1.0;
        curSpeed = moveSpeed;
        if(traveling_through != null)
        {
            curSpeed -= traveling_through.currentSpeed; // * currentCoeff;
        }
    }

    public List<CrewUISlot> GetCrewSlots()
    {
        return availableCrew;
    }

    public Transform GetCrewRestTransform()
    {
        return transform;
    }

    public void UpdateCrewSlots()
    {
        availableCrew = new List<CrewUISlot>();
        for(int i = 0; i < crewSeats; i++)
        {
            CrewUISlot crewSlot = Instantiate(Game.inst.crewSlotUI_prefab, transform).GetComponent<CrewUISlot>();
            availableCrew.Add(crewSlot);
        }
    }

    public void Enter(HyperspaceCorridor corridor, Star destination)
    {
        corridor.BeforeEnter(this);

        traveling_through = corridor;
    
        SetTarget(destination);
        UnsetOrbiting();

        UpdateSpeed();

        corridor.AfterEnter(this);
    }

    public void Exit()
    {
        traveling_through.BeforeExit(this);

        SetOrbiting(target);
        HyperspaceCorridor corridor = traveling_through;
        traveling_through = null;

        corridor.AfterExit(this);
    }

    public void SetOrbiting(Star new_orbiting)
    {
        orbiting = new_orbiting;
        transform.localPosition = new_orbiting.pos + Vector3.up * hoverDistance + Vector3.up * orbiting.size;
    
        my_orbit.rotating = true;
        my_orbit.centerPoint = orbiting.transform;
        my_orbit.xSpread = orbitDistance + orbiting.size;
        my_orbit.zSpread = orbitDistance + orbiting.size;
        my_orbit.yOffset = hoverDistance + orbiting.size;
        my_orbit.rotateClockwise = Random.Range(0, 2) == 0 ? false : true;

        state = ShipState.Idle;

        UnsetTarget();
    }

    public void UnsetOrbiting()
    {
        orbiting = null;
        state = ShipState.Flying;
        my_orbit.rotating = false;
        my_orbit.centerPoint = null;
    }

    public void SetTarget(Star new_target)
    {
        if(state != ShipState.Idle)
        {
            return;
        }

        target = new_target;
        my_orbit.rotating = false;
        target_pos = new_target.pos + Vector3.up * hoverDistance + Vector3.up * new_target.size;

        state = ShipState.Flying;
    }

    public void UnsetTarget()
    {
        target = null;
        state = ShipState.Idle;
    }

    public override void BeforeSelect(Player player)
    {
        my_renderer.material.SetColor("_Color", Color.red);
    }

    public override void AfterUnselect(Player player)
    {
        UIManager.inst.ShipUIOff(this);
        my_renderer.material.SetColor("_Color", Color.white);
    }
}
