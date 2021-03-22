using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public abstract class IModularPreset<TModular, TSlot, TModule, TKeyword>: ScriptableObject, ISerializationCallbackReceiver
    where TModular : MonoBehaviour
    where TSlot : IModuleSlot<TModular, TKeyword>
    where TModule : IModule<TModular, TKeyword>
{
    [SerializeField]
    public List<TSlot> slots;

    [System.Serializable]
    public class SlottedModule<OTModule> where OTModule : TModule
    {
        public string slot_name;
        public OTModule module;
    }

    [SerializeField]
    private List<SlottedModule<TModule>> modules_by_slot;

    public Dictionary<string, TModule> modules;

    void Awake()
    {
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        modules = new Dictionary<string, TModule>();
        foreach(SlottedModule<TModule> ssm in modules_by_slot)
        {
            modules[ssm.slot_name] = ssm.module;
        }
    }
}
