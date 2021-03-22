using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IModule<TModular, TKeyword> : ScriptableObject, ISerializationCallbackReceiver
    where TModular : MonoBehaviour
{
    [HideInInspector]
    public string takenSlotName;

    [SerializeField]
    private List<TKeyword> set_keywords;
    public HashSet<TKeyword> keywords;

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        keywords = new HashSet<TKeyword>(set_keywords);
    }

    public abstract void Install(TModular affected);
    public abstract void Uninstall(TModular affected);
}
