using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public abstract class IModuleHolder<TModular, TSlot, TModule, TKeyword> : MonoBehaviour
    where TModular : MonoBehaviour
    where TSlot : IModuleSlot<TModular, TKeyword>
    where TModule : IModule<TModular, TKeyword>
{
    TModular affected;

    List<TSlot> available_slots = new List<TSlot>();

    Dictionary<string, TSlot> slots_by_name = new Dictionary<string, TSlot>();

    Dictionary<string, TModule> modules_by_name = new Dictionary<string, TModule>();

    Dictionary<string, int> slot_amount = new Dictionary<string, int>();

    void Awake()
    {
        affected = GetComponent<TModular>();
    }

    public void Init(IModularPreset<TModular, TSlot, TModule, TKeyword> preset)
    {
        if(preset == null)
        {
            return;
        }

        AddSlots(preset.slots);
        AttachTModules(preset.modules);
    }

    void OnDestroy()
    {
        affected = null;
    }

    public void AddSlots(List<TSlot> slots)
    {
        foreach(TSlot slot in slots)
        {
            AddSlot(slot);
        }
    }

    public void AddSlot(TSlot slot)
    {
        available_slots.Add(slot);

        if(!slot_amount.ContainsKey(slot.name))
        {
            slot_amount[slot.name] = 0;
        }
        slot_amount[slot.name] += 1;

        slot.fullName = slot.name + " " + slot_amount[slot.name];
        slots_by_name[slot.fullName] = slot;
    }

    public void RemoveSlot(TSlot slot)
    {
        DetachTModule(modules_by_name[slot.name]);
        available_slots.Remove(slot);
        slots_by_name.Remove(slot.fullName);

        slot_amount[slot.name] = 0;

        foreach(TSlot other in available_slots)
        {
            if(other.name == slot.name)
            {
                slots_by_name.Remove(other.fullName);
                slot_amount[other.name] += 1;
                other.fullName = slot.name + " " + slot_amount[slot.name];
                slots_by_name[slot.fullName] = slot;
            }
        }
    }

    public void AttachAnywhere(TModule TModule)
    {
        foreach(TSlot slot in available_slots)
        {
            if(slot.IsAllowed(TModule))
            {
                AttachTModule(TModule, slot);
                return;
            }
        }
    }

    public void AttachTModules(Dictionary<string, TModule> modules)
    {
        foreach(KeyValuePair<string, TModule> slot_module in modules)
        {
            AttachTModule(slot_module.Value, slots_by_name[slot_module.Key]);
        }
    }

    public void AttachTModule(TModule module, TSlot slot)
    {
        available_slots.Remove(slot);
        modules_by_name[slot.name] = module;

        module.takenSlotName = slot.name;

        module.Install(affected);
    }

    public void DetachTModule(TModule module)
    {
        available_slots.Add(slots_by_name[module.takenSlotName]);
        modules_by_name.Remove(module.takenSlotName);

        module.takenSlotName = "";

        module.Uninstall(affected);
    }
}
